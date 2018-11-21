using System;
using Microsoft.AspNetCore.Mvc;
using WebAssistedSurvey.Business;
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
            var surveyEvent = RestAdapter.GetEventById(id);
            if (surveyEvent == null)
            {
                return new StatusCodeResult(503);
            }

            if (RestAdapter.IsValidEventExisting(id))
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

            RestAdapter.AddSurvey(survey);

            return Content("Danke für Ihre Mithilfe.");
        }
    }
}