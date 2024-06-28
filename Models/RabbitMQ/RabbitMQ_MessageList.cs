namespace KevDevTools.Models.RabbitMQ
{
    public class RabbitMQ_MessageList
    {
        public List<RabbitMQ_MessageObj> Messages { get; set; } = new List<RabbitMQ_MessageObj>();
    }

    public class RabbitMQ_MessageObj
    {
        public string Message { get; set; }
        public string SessionId { get; set; }
    }
}
