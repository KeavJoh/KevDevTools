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
            return base.OnDisconnectedAsync(exception);
        }

        public Task<RabbitMQ_ConnectionObj> CreateRabbitConnection(RabbitMQ_ConnectionObj connectionObj)
        {
            Console.WriteLine("CreateRabbitConnection");
            var connectionId = Context.ConnectionId;
            connectionObj.ConnectionId = connectionId;
            connectionObj = _rabbitMQService.Initialize(connectionObj);
            return Task.FromResult(connectionObj);
        }


    }
}
