namespace KevDevTools.Models.RabbitMQ
{
    public class RabbitMQ_ConnectionObj
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public int Port { get; set; }
        public string QueueName { get; set; }
        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }
        public bool Connected { get; set; }
        public string SessionId { get; set; }
        public string ConnectionId { get; set; }
    }
}
