using KevDevTools.Models.RabbitMQ;
using NuGet.Protocol.Plugins;
using RabbitMQ.Client;
using IConnection = RabbitMQ.Client.IConnection;

namespace KevDevTools.Services
{
    public class RabbitMQService
    {
        private readonly Dictionary<string, IConnection> _connections = new Dictionary<string, IConnection>();
        private readonly Dictionary<string, IModel> _channels = new Dictionary<string, IModel>();

        public void Initialize(string sessionId ,RabbitMQ_ConnectionObj rabbitObj)
        {

            if (_connections.ContainsKey(sessionId))
            {
                _connections[sessionId].Close();
                _connections.Remove(sessionId);
            }

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

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: rabbitObj.QueueName,
                durable: rabbitObj.Durable,
                exclusive: rabbitObj.Exclusive,
                autoDelete: rabbitObj.AutoDelete);

            _connections[sessionId] = connection;
            _channels[sessionId] = channel;
        }

        public IModel GetChannel(string sessionId)
        {
            if (!_channels.ContainsKey(sessionId))
                throw new InvalidOperationException("RabbitMQService is not initialized for this session.");

            return _channels[sessionId];
        }


        public void Dispose(string sessionId)
        {
            if (_channels.ContainsKey(sessionId))
            {
                _channels[sessionId].Close();
                _channels.Remove(sessionId);
            }

            if (_connections.ContainsKey(sessionId))
            {
                _connections[sessionId].Close();
                _connections.Remove(sessionId);
            }
        }
    }
}
