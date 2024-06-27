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
            if (RabbitMQConnectionExists())
            {
                rabbitObj = GetRabbitMQConnectionObjFromSession();
            }
            ViewBag.RabbitObj = rabbitObj;
            return View();
        }

        public IActionResult ConnectToRabbitService(RabbitMQ_ConnectionObj rabbitObj)
        {
            try
            {
                string sesionId = HttpContext.Session.Id;
                _rabbitMQService.Initialize(sesionId, rabbitObj);
                rabbitObj.Connected = true;
                HttpContext.Session.SetString("RabbitMQ_ConnectionObj", Newtonsoft.Json.JsonConvert.SerializeObject(rabbitObj));
            } catch
            {
                rabbitObj.Connected = false;
            }

            return RedirectToAction("RabbitMQ_Tool");
        }

        private RabbitMQ_ConnectionObj GetRabbitMQConnectionObjFromSession()
        {
            var rabbitObjString = HttpContext.Session.GetString("RabbitMQ_ConnectionObj");
            if (!string.IsNullOrEmpty(rabbitObjString))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<RabbitMQ_ConnectionObj>(rabbitObjString);
            }
            return new RabbitMQ_ConnectionObj();
        }

        private bool RabbitMQConnectionExists()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("RabbitMQ_ConnectionObj"));
        }

        public IActionResult CloseRabbitConnection()
        {
            var rabbitObj = GetRabbitMQConnectionObjFromSession();
            HttpContext.Session.Remove("RabbitMQ_ConnectionObj");
            _rabbitMQService.Dispose(HttpContext.Session.Id);
            rabbitObj.Connected = false;
            return RedirectToAction("RabbitMQ_Tool", rabbitObj);
        }
    }
}
