using KevDevTools.Models.RabbitMQ;
using KevDevTools.Services;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace KevDevTools.Controllers
{
    public class RabbitMQController : Controller
    {
        private readonly RabbitMQService _rabbitMQService;
        private RabbitMQ_MessageList _rabbitMQ_MessageList;

        public RabbitMQController(RabbitMQService rabbitMQService, RabbitMQ_MessageList rabbitMQ_MessageList)
        {
            _rabbitMQService = rabbitMQService;
            _rabbitMQ_MessageList = rabbitMQ_MessageList;
        }

        public IActionResult RabbitMQ_Tool()
        {
            string sesionId = HttpContext.Session.Id;
            Console.WriteLine("SessionId: " + sesionId);
            ViewBag.SessionId = sesionId;
            return View();
        }

        //public IActionResult ConnectToRabbitService(RabbitMQ_ConnectionObj rabbitObj)
        //{
        //    try
        //    {
        //        string sesionId = HttpContext.Session.Id;
        //        rabbitObj.SessionId = sesionId;
        //        Console.WriteLine("SessionId: " + sesionId);
        //        _rabbitMQService.Initialize(sesionId, rabbitObj);
        //        rabbitObj.Connected = true;
        //        HttpContext.Session.SetString("RabbitMQ_ConnectionObj", Newtonsoft.Json.JsonConvert.SerializeObject(rabbitObj));
        //    } catch
        //    {
        //        rabbitObj.Connected = false;
        //    }

        //    return RedirectToAction("RabbitMQ_Tool");
        //}

        //public IActionResult SendMessageToRabbitMQ(string message)
        //{
        //    var rabbitObj = GetRabbitMQConnectionObjFromSession();
        //    _rabbitMQService.SendMessage(HttpContext.Session.Id, message, rabbitObj.QueueName);
        //    return RedirectToAction("ReceiveMessages");
        //}

        //public IActionResult ReceiveMessages()
        //{
        //    var rabbitObj = GetRabbitMQConnectionObjFromSession();
        //    var messages = _rabbitMQ_MessageList.Messages.Where(x => x.SessionId == HttpContext.Session.Id).Select(x => x.Message).ToList();
        //    rabbitObj.Messages.Clear();
        //    foreach (var message in messages)
        //    {
        //        rabbitObj.Messages.Add(message);
        //    }
        //    HttpContext.Session.Remove("RabbitMQ_ConnectionObj");
        //    HttpContext.Session.SetString("RabbitMQ_ConnectionObj", Newtonsoft.Json.JsonConvert.SerializeObject(rabbitObj));
        //    return RedirectToAction("RabbitMQ_Tool");
        //}

        //private RabbitMQ_ConnectionObj GetRabbitMQConnectionObjFromSession()
        //{
        //    var rabbitObjString = HttpContext.Session.GetString("RabbitMQ_ConnectionObj");
        //    if (!string.IsNullOrEmpty(rabbitObjString))
        //    {
        //        return Newtonsoft.Json.JsonConvert.DeserializeObject<RabbitMQ_ConnectionObj>(rabbitObjString);
        //    }
        //    return new RabbitMQ_ConnectionObj();
        //}

        //private bool RabbitMQConnectionExists()
        //{
        //    return !string.IsNullOrEmpty(HttpContext.Session.GetString("RabbitMQ_ConnectionObj"));
        //}

        //private bool RabbitMQMessageExists()
        //{
        //    return !string.IsNullOrEmpty(HttpContext.Session.GetString("RabbitMQ_Message"));
        //}

        //private string GetRabbitMQMessageFromSession()
        //{
        //    return HttpContext.Session.GetString("RabbitMQ_Message");
        //}

        //public IActionResult CloseRabbitConnection()
        //{
        //    var rabbitObj = GetRabbitMQConnectionObjFromSession();
        //    HttpContext.Session.Remove("RabbitMQ_ConnectionObj");
        //    _rabbitMQService.Dispose(HttpContext.Session.Id);
        //    rabbitObj.Connected = false;
        //    return RedirectToAction("RabbitMQ_Tool", rabbitObj);
        //}

    }
}
