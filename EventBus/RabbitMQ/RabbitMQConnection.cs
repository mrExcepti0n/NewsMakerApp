using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.RabbitMQ
{
    public class RabbitMQConnection
    {
        private IConnection _connection;
        private bool _disposed;
        private readonly IConnectionFactory _connectionFactory;

        public RabbitMQConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

        public IModel CreateModel()
        {
            return _connection.CreateModel();
        }      


        public bool TryConnect()
        {
            _connection = _connectionFactory.CreateConnection();
            return IsConnected;
        }

        
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;            
            _connection.Dispose();            
        }

    }
}
