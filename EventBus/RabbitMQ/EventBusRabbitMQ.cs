using Autofac;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : IDisposable
    {
        private const string ExchangeName = "billing_event_bus";


        private readonly RabbitMQConnection _connection;

       // private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly ILifetimeScope _autofac;
        private readonly string AUTOFAC_SCOPE_NAME = "billing_event_bus";
        private readonly int _retryCount;

        private IModel _consumerChannel;
        private string _queueName;

        public EventBusRabbitMQ(RabbitMQConnection connection, ILifetimeScope autofac,/* IEventBusSubscriptionsManager subsManager,*/     string queueName = null)
        {

            _connection = connection;
            //_subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();
            _queueName = queueName /*?? ConfigurationManager.AppSettings["SubscriptionClientName"]*/;

            _autofac = autofac;
            SetConsumerChannel();
        }

        public void Publish(IntegrationEvent @event, string correlationId = null)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }
            using (var channel = _persistentConnection.CreateModel())
            {
                var eventName = @event.GetType().Name;
                channel.ExchangeDeclare(exchange: ExchangeName, type: "direct", durable: true);
                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; //persistent

                channel.BasicPublish(exchange: ExchangeName,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body);
            }
        } 

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();
            DoInternalSubscription(eventName);
            _subsManager.AddSubscription<T, TH>();
        }

        private void DoInternalSubscription(string eventName)
        {
            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }

                using (var channel = _persistentConnection.CreateModel())
                {
                    channel.QueueBind(queue: _queueName,
                                      exchange: ExchangeName,
                                      routingKey: eventName);
                }
            }
        }

        public void Unsubscribe<T, TH>()
            where TH : IntegrationEventHandler<T>
            where T : IntegrationEvent
        {
            _subsManager.RemoveSubscription<T, TH>();
        }

        public void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.RemoveDynamicSubscription<TH>(eventName);
        }

        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }
        }

        private void SetConsumerChannel()
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            _consumerChannel = _connection.CreateModel();

            _consumerChannel.ExchangeDeclare(exchange: ExchangeName, type: "direct", durable: true);

            _consumerChannel.BasicQos(0, 20, false);

            _consumerChannel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(_consumerChannel);

            consumer.Received += Consumer_Recieved;
            _consumerChannel.BasicConsume(queue: _queueName,
                                 autoAck: false,
                                 consumer: consumer); 
        }


        private async void Consumer_Recieved(object sender, BasicDeliverEventArgs ea)
        {
            var eventName = ea.RoutingKey;
            var message = Encoding.UTF8.GetString(ea.Body);
            var props = ea.BasicProperties;


            bool isProcessedEvent;

            if (string.IsNullOrEmpty(props.ReplyTo))
            {
                isProcessedEvent = await ProcessEvent(eventName, message);
            }
            else
            {
                OperationResult result = await ProcessRpcEvent(eventName, message);

                isProcessedEvent = result.Success;

                if (isProcessedEvent)
                {
                    var replyProps = _consumerChannel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;
                    string response = JsonConvert.SerializeObject(result);
                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    _consumerChannel.BasicPublish(exchange: ResponseExchangeName, routingKey: props.ReplyTo,
                        basicProperties: replyProps, body: responseBytes);
                }
            }

            if (isProcessedEvent)
            {
                _consumerChannel.BasicAck(ea.DeliveryTag, multiple: false);
            }
            else
            {
                Thread.Sleep(1000);
                _consumerChannel.BasicReject(ea.DeliveryTag, true);
            }
        }

     



        private async Task<bool> ProcessEvent(string eventName, string message)
        {
            if (!_subsManager.HasSubscriptionsForEvent(eventName))
            {
                return false;
            }

            try
            {
                using (var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
                {
                    var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions)
                    {
                        if (subscription.IsDynamic)
                        {
                            var handler =
                                scope.ResolveOptional(subscription.HandlerType) as IDynamicIntegrationEventHandler;
                            if (handler == null) continue;
                            dynamic eventData = JObject.Parse(message);
                            await handler.Handle(eventData);
                        }
                        else
                        {
                            var handler = scope.ResolveOptional(subscription.HandlerType);
                            if (handler == null) continue;
                            var eventType = _subsManager.GetEventTypeByName(eventName);
                            var integrationEvent =
                                JsonConvert.DeserializeObject(message, eventType) as IntegrationEvent;
                            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                            OperationResult result = await (Task<OperationResult>)
                                concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Ошибка при обработки очереди");
                return true;
            }
            return true;
        }


        private async Task<OperationResult> ProcessRpcEvent(string eventName, string message)
        {
            if (!_subsManager.HasSubscriptionsForEvent(eventName))
            {
                return OperationResult.CreateErrorResult();
            }

            try
            {
                using (var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
                {
                    var subscriptions = _subsManager.GetHandlersForEvent(eventName).ToArray();

                    if (subscriptions.Length > 1)
                    {
                        throw new Exception("Подписчиков > 1");
                    }

                    var subscription = subscriptions.Single();
                    var handler = scope.ResolveOptional(subscription.HandlerType);
                    if (handler == null) return OperationResult.CreateErrorResult();
                    var eventType = _subsManager.GetEventTypeByName(eventName);
                    var integrationEvent =
                        JsonConvert.DeserializeObject(message, eventType) as IntegrationEvent;
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                    OperationResult result = await (Task<OperationResult>)concreteType.GetMethod("Handle")
                        .Invoke(handler, new object[] { integrationEvent });
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Ошибка при обработки очереди");
            }
            return OperationResult.CreateErrorResult();
        }
    }
}
