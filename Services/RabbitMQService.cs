using KevDevTools.Hubs;
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
        private readonly Dictionary<string, IConnection> _connections = new Dictionary<string, IConnection>();
        private readonly Dictionary<string, IModel> _channels = new Dictionary<string, IModel>();
        private RabbitMQ_MessageList _rabbitMQ_MessageList;
        private readonly IHubContext<ViewCounterHub> _viewCounterHub;

        public RabbitMQService(RabbitMQ_MessageList rabbitMQ_MessageList, IHubContext<ViewCounterHub> viewCounterHub)
        {
            _rabbitMQ_MessageList = rabbitMQ_MessageList;
            _viewCounterHub = viewCounterHub;
        }

        public RabbitMQ_ConnectionObj Initialize(RabbitMQ_ConnectionObj rabbitObj)
        {
            try
            {
                if (_connections.ContainsKey(rabbitObj.SessionId))
                {
                    _connections[rabbitObj.SessionId].Close();
                    _connections.Remove(rabbitObj.SessionId);
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

                _connections[rabbitObj.SessionId] = connection;
                _channels[rabbitObj.SessionId] = channel;

                Task.Run(() => ReceiveMessages(channel, rabbitObj.SessionId, rabbitObj.QueueName, rabbitObj.ConnectionId));

                rabbitObj.Connected = true;

                return rabbitObj;
            } catch
            {
                rabbitObj.Connected = false;

                return rabbitObj;
            }
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

        private async Task ReceiveMessages(IModel channel, string sessionId, string queueName, string connectionId)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _rabbitMQ_MessageList.Messages.Add(new RabbitMQ_MessageObj { Message = message, SessionId = sessionId });
                await _viewCounterHub.Clients.All.SendAsync("ReceiveMessage", message);
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
