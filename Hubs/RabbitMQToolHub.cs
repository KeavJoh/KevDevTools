using KevDevTools.Models.RabbitMQ;
using KevDevTools.Services;
using Microsoft.AspNetCore.SignalR;

namespace KevDevTools.Hubs
{
    public class RabbitMQToolHub : Hub
    {
        private readonly RabbitMQService _rabbitMQService;

        public RabbitMQToolHub(RabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            DeleteRabbitConnection();
            return base.OnDisconnectedAsync(exception);
        }

        public Task<RabbitMQ_ConnectionObj> CreateRabbitConnection(RabbitMQ_ConnectionObj connectionObj)
        {
            var connectionId = Context.ConnectionId;
            RabbitMQConnectionDictionary.rabbitConnectionIds.Remove(connectionId);
            RabbitMQConnectionDictionary.rabbitConnectionIds.Add(connectionId, connectionObj.SessionId);
            connectionObj.ConnectionId = connectionId;
            connectionObj = _rabbitMQService.Initialize(connectionObj);
            return Task.FromResult(connectionObj);
        }

        public Task DeleteRabbitConnection()
        {
            var connectionId = Context.ConnectionId;
            if (RabbitMQConnectionDictionary.rabbitConnectionIds.TryGetValue(connectionId, out string value))
            {
                _rabbitMQService.Dispose(value);
                RabbitMQConnectionDictionary.rabbitConnectionIds.Remove(connectionId);
            }
            return Task.CompletedTask;
        }


    }
}
