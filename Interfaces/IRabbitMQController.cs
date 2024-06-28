namespace KevDevTools.Interfaces
{
    public interface IRabbitMQController
    {
        void ReceiveMessages(string message, string sessionId);
    }
}
