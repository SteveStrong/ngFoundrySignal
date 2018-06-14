using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ngFoundrySignal
{

    public class HomeController : Controller
    {
        [Route("home/index")]

        public IActionResult Index()
        {
            return Redirect(Url.Content("~/index.html"));
        }

        public IActionResult Start()
        {
            return Redirect(Url.Content("~/Start.html"));
        }

        public IActionResult Doc(string id)
        {
            if (id.EndsWith("knt", true, System.Globalization.CultureInfo.DefaultThreadCurrentCulture))
            {
                return Redirect(Url.Content("~/Diagram.html?doc=" + id));
            }

            return Index();
        }

        public IActionResult KnowtShare(string id)
        {
            return Redirect(Url.Content("~/KnowtView.html?session=" + id));
        }

        public IActionResult Knowtify(string id)
        {
            return Redirect(Url.Content("~/Knowtify.html?session=" + id));
        }

        public IActionResult Diagram()
        {
            return Redirect(Url.Content("~/Diagram.html"));
        }



        public IActionResult Invitations(string id)
        {
            return Redirect(Url.Content("~/Invitations.html?id=" + id));
        }

        public IActionResult Accentances(string id)
        {
            return Redirect(Url.Content("~/Accentances.html?id=" + id));
        }

    }
}