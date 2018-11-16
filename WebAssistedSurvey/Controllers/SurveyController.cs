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
            var surveyEvent = DatabaseLayer.GetEventById(id);
            if (surveyEvent == null)
            {
                return NotFound();
            }

            if (DatabaseLayer.IsValidEventExisting(id))
            {
                var survey = DatabaseLayer.CreateNewSurvey(id);
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

            if(!DatabaseLayer.AddSurvey(survey))
            {
                return NotFound();
            }

            return Content("Danke für Ihre Mithilfe.");
        }
    }
}