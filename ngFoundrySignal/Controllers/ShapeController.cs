using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;


//https://blogs.msdn.microsoft.com/webdev/2017/09/14/announcing-signalr-for-asp-net-core-2-0/


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ngFoundrySignal
{
    [Route("[controller]")]
    [ApiController]
    public class ShapeController : Controller
    {

        private IHubContext<ShapeHub> _shapeHubContext;
        public ShapeController(IHubContext<ShapeHub> context)
        {
            _shapeHubContext = context;
        }
        [HttpGet("SayHello")]
        public IActionResult SayHello()
        {
            //broadcast message to chat
            _shapeHubContext.Clients.All.SendAsync("send", "Hello from the server");
            return Ok();
        }
        [HttpGet("{message}")]
        public IActionResult Send(string message)
        {
            //broadcast message to chat
            _shapeHubContext.Clients.All.SendAsync("send", message);
            return Ok();
        }

        // POST api/values
        [HttpPost]
        public IActionResult SendAll([FromBody]string message)
        {
            //broadcast message to chat
            _shapeHubContext.Clients.All.SendAsync("send", message);
            return Ok();
        }
    }
}
