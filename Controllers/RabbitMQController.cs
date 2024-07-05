using KevDevTools.Models.RabbitMQ;
using KevDevTools.Services;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace KevDevTools.Controllers
{
    public class RabbitMQController : Controller
    {
        private readonly RabbitMQService _rabbitMQService;

        public RabbitMQController(RabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }

        public IActionResult RabbitMQ_Tool()
        {
            string sesionId = HttpContext.Session.Id;
            Console.WriteLine("SessionId: " + sesionId);
            ViewBag.SessionId = sesionId;
            return View();
        }

    }
}
