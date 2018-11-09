using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAssistedSurvey.Models;

namespace WebAssistedSurvey.Controllers
{
    public class SurveyController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Message = "Hier gibt es aktuell nichts zu sehen ...";

            return View();
        }

        public IActionResult Show(int id)
        {
            var context = new SurveyContext();
            
            var surveyEvent = context.Events.FirstOrDefault(e => e.EventID == id);
            if (surveyEvent == null)
            {
                return NotFound();
            }

            var now = DateTime.Now;
            if (surveyEvent.StartDateTime.CompareTo(now) == -1 &&
                surveyEvent.EndDateTime.GetValueOrDefault().CompareTo(now) == 1)
            {
                Survey survey = new Survey
                {
                    Event = surveyEvent,
                    EventID = surveyEvent.EventID,
                    Created = DateTime.Now
                };

                return View(survey);
            }

            return View();
        }

        public IActionResult Add(Survey survey)
        {
            if (!TryValidateModel(survey))
            {
                return View("Show", survey);
            }

            var context = new SurveyContext();

            var surveyEvent = context.Events.FirstOrDefault(e => e.EventID == survey.EventID);
            if (surveyEvent == null)
            {
                return NotFound();
            }

            context.Surveys.Add(survey);
            context.SaveChanges();

            return Content("Danke für Ihre Mithilfe.");
        }
    }
}