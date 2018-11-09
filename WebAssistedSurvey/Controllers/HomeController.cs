using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAssistedSurvey.Models;

namespace WebAssistedSurvey.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var context = new SurveyContext();

            ViewBag.DateTimeNow = DateTime.Now.ToString();

            return View(context.Events.ToList());
        }
    }
}
