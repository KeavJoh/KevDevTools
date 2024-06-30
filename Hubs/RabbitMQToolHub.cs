using Microsoft.AspNetCore.SignalR;

namespace KevDevTools.Hubs
{
    public class RabbitMQToolHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public Task CreateRabbitConnection(RabbitConnectionData connectionData)
        {
            Console.WriteLine("CreateRabbitConnection");
            return Task.CompletedTask;
        }


    }

    public class RabbitConnectionData
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public int Port { get; set; }
        public string QueName { get; set; }
        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }
    }
}
