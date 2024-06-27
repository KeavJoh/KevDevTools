using KevDevTools.Models.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace KevDevTools.Controllers
{
    public class RabbitMQController : Controller
    {
        public IActionResult RabbitMQ_Tool(RabbitMQ_ConnectionObj rabbitObj)
        {
            ViewBag.RabbitObj = rabbitObj;
            return View();
        }

        public IActionResult ConnectToRabbitService(RabbitMQ_ConnectionObj rabbitObj)
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
            try
            {
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: rabbitObj.QueueName,
                    durable: rabbitObj.Durable,
                    exclusive: rabbitObj.Exclusive,
                    autoDelete: rabbitObj.AutoDelete);

                rabbitObj.Connected = true;
            } catch (Exception ex)
            {
                rabbitObj.Connected = false;
            }

            return RedirectToAction("RabbitMQ_Tool", rabbitObj);
        }
    }
}
