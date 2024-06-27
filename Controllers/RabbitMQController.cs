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

        public IActionResult RabbitMQ_Tool(RabbitMQ_ConnectionObj rabbitObj)
        {
            ViewBag.RabbitObj = rabbitObj;
            return View();
        }

        public IActionResult ConnectToRabbitService(RabbitMQ_ConnectionObj rabbitObj)
        {
            try
            {
                _rabbitMQService.Initialize(rabbitObj);
                var channel = _rabbitMQService.GetChannel();
                rabbitObj.Connected = true;
            } catch
            {
                rabbitObj.Connected = false;
            }

            return RedirectToAction("RabbitMQ_Tool", rabbitObj);
        }
    }
}
