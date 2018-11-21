using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using WebAssistedSurvey.Business;

namespace WebAssistedSurvey.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.DateTimeNow = DateTime.Now.ToString();

            var events = RestAdapter.GetEvents();
            if (events == null)
            {
                return StatusCode(503);
            }

            return View(events);
        }
    }
}
