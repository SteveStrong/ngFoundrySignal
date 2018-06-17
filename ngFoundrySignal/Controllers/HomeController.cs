using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// https://docs.microsoft.com/en-us/aspnet/core/aspnetcore-2.1?view=aspnetcore-2.1


namespace ngFoundrySignal
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        [HttpGet("index")]
        public IActionResult Index()
        {
            return Redirect(Url.Content("~/index.html"));
        }

        [HttpGet("start")]
        public IActionResult Start()
        {
            return Redirect(Url.Content("~/Start.html"));
        }

        [HttpGet("doc/{id}")]
        public IActionResult Doc(string id)
        {
            if (id.EndsWith("knt", true, System.Globalization.CultureInfo.DefaultThreadCurrentCulture))
            {
                return Redirect(Url.Content("~/Diagram.html?doc=" + id));
            }

            return Index();
        }
        [HttpGet("knowtshare/{id}")]
        public IActionResult KnowtShare(string id)
        {
            return Redirect(Url.Content("~/KnowtView.html?session=" + id));
        }

        // public IActionResult Knowtify(string id)
        // {
        //     return Redirect(Url.Content("~/Knowtify.html?session=" + id));
        // }

        // public IActionResult Diagram()
        // {
        //     return Redirect(Url.Content("~/Diagram.html"));
        // }


        // public IActionResult Invitations(string id)
        // {
        //     return Redirect(Url.Content("~/Invitations.html?id=" + id));
        // }

        // public IActionResult Accentances(string id)
        // {
        //     return Redirect(Url.Content("~/Accentances.html?id=" + id));
        // }

    }
}