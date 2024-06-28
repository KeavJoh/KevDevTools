using KevDevTools.Controllers;
using KevDevTools.Interfaces;
using KevDevTools.Models.RabbitMQ;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using IConnection = RabbitMQ.Client.IConnection;

namespace KevDevTools.Services
{
    public class RabbitMQService
    {
        private readonly IHubContext<MessageHubService> _hubContext;
        private readonly Dictionary<string, IConnection> _connections = new Dictionary<string, IConnection>();
        private readonly Dictionary<string, IModel> _channels = new Dictionary<string, IModel>();
        private RabbitMQ_MessageList _rabbitMQ_MessageList;

        public RabbitMQService(IHubContext<MessageHubService> hubContext, RabbitMQ_MessageList rabbitMQ_MessageList)
        {
            _hubContext = hubContext;
            _rabbitMQ_MessageList = rabbitMQ_MessageList;
        }

        public void Initialize(string sessionId, RabbitMQ_ConnectionObj rabbitObj)
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

            Task.Run(() => ReceiveMessages(channel, sessionId, rabbitObj.QueueName));
        }

        public void SendMessage(string sessionId, string message, string queueName)
        {
            var channel = GetChannel(sessionId);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: string.Empty,
                               routingKey: queueName,
                               basicProperties: null,
                               body: body);
            Task.Delay(2000).Wait();
        }

        private async Task ReceiveMessages(IModel channel, string sessionId, string queueName)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _rabbitMQ_MessageList.Messages.Add(new RabbitMQ_MessageObj { Message = message, SessionId = sessionId });
            };

            channel.BasicConsume(queue: queueName,
                autoAck: true,
                consumer: consumer);

            await Task.Delay(-1);
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
