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

       private readonly SubscriptionsManager _subsManager;
        private readonly ILifetimeScope _autofac;
        private readonly string AUTOFAC_SCOPE_NAME = "billing_event_bus";
        private readonly int _retryCount;

        private IModel _consumerChannel;
        private string _queueName;

        public EventBusRabbitMQ(RabbitMQConnection connection, ILifetimeScope autofac,  string queueName = null)
        {

            _connection = connection;           
            _queueName = "NewsQ";

            _subsManager = new SubscriptionsManager();

            _autofac = autofac;
            SetConsumerChannel();
        }

        public void Publish(IntegrationEvent @event)
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }
            using (var channel = _connection.CreateModel())
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
            where TH : IIntegrationEventHandler<T>
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
                if (!_connection.IsConnected)
                {
                    _connection.TryConnect();
                }

                using (var channel = _connection.CreateModel())
                {
                    channel.QueueBind(queue: _queueName,
                                      exchange: ExchangeName,
                                      routingKey: eventName);
                }
            }
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


            bool isProcessedEvent = await ProcessEvent(eventName, message); 
            _consumerChannel.BasicAck(ea.DeliveryTag, multiple: false);
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
                    foreach (var subscriptionType in subscriptions)
                    {
                        var handler = scope.ResolveOptional(subscriptionType);
                        if (handler == null) continue;
                        var eventType = _subsManager.GetEventTypeByName(eventName);
                        var integrationEvent =
                                JsonConvert.DeserializeObject(message, eventType) as IntegrationEvent;
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                        await (Task) concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                     }
                    
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
