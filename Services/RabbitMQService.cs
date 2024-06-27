using KevDevTools.Models.RabbitMQ;
using NuGet.Protocol.Plugins;
using RabbitMQ.Client;
using IConnection = RabbitMQ.Client.IConnection;

namespace KevDevTools.Services
{
    public class RabbitMQService : IDisposable
    {
        private IConnection _connection;
        private IModel _channel;

        public void Initialize(RabbitMQ_ConnectionObj rabbitObj)
        {
            var factory = new ConnectionFactory()
            {
                HostName = rabbitObj.HostName,
                UserName = rabbitObj.UserName,
                Password = rabbitObj.Password,
                VirtualHost = rabbitObj.VirtualHost,
                Port = rabbitObj.Port,
                Ssl = new SslOption()
                {
                    Enabled = true,
                    ServerName = rabbitObj.HostName
                }
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: rabbitObj.QueueName,
                durable: rabbitObj.Durable,
                exclusive: rabbitObj.Exclusive,
                autoDelete: rabbitObj.AutoDelete);
        }

        public IModel GetChannel() => _channel;

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
