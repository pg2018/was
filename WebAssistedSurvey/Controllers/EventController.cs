using System.Collections.Generic;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Mvc;
using WebAssistedSurvey.Business;
using WebAssistedSurvey.Models;

namespace WebAssistedSurvey.Controllers
{
    public class EventController : Controller
    {
        public IActionResult Index()
        {
            var events = RestAdapter.GetEvents();
            if (events == null)
            {
                return new StatusCodeResult(503);
            }

            return View(events);
        }

        public IActionResult New()
        {
            Event newEvent = DatabaseLayer.CreateNewEvent();

            return View(newEvent);
        }

        public IActionResult Add(Event @event)
        {
            if (!TryValidateModel(@event))
            {
                return View("New", @event);
            }

            RestAdapter.AddEvent(@event, 3);

            return RedirectToAction("Index");
        }

        public IActionResult Show(int id)
        {
            Event surveyEvent = RestAdapter.GetEventById(id);
            if (surveyEvent == null)
            {
                return new StatusCodeResult(503);
            }

            return View(surveyEvent);
        }

        public IActionResult GetSurveysAsCsv(int id)
        {
            Event surveyEvent = RestAdapter.GetEventById(id);
            if (surveyEvent == null)
            {
                return new StatusCodeResult(503);
            }

            if(surveyEvent.Surveys == null)
            {
                TempData["CsvMessage"] = $"Für die Veranstaltung \"{surveyEvent.Title}\" wurden keine Umfragen zum exportieren gefunden.";
                var events = RestAdapter.GetEvents();
                if (events == null)
                {
                    return new StatusCodeResult(503);
                }

                return View("Index", events);
            }

            var data = CsvExporter.GetCsvData(surveyEvent);
            return File(data, "text/csv", $"Umfrage_Antworten_{id}.csv");
        }

        public IActionResult ShowSurvey(int id)
        {
            Survey survey = RestAdapter.GetSurveyById(id);
            if (survey == null)
            {
                return new StatusCodeResult(503);
            }

            return View(survey);
        }

        public IActionResult DeleteSurvey(int id)
        {
            Survey survey = DatabaseLayer.GetSurveyById(id);
            if (survey == null)
            {
                return new StatusCodeResult(503);
            }

            return View(survey);
        }

        public IActionResult Delete(int id)
        {
            var eventToDelete = RestAdapter.GetEventById(id);
            if (eventToDelete == null)
            {
                return new StatusCodeResult(503);
            }

            return View(eventToDelete);
        }

        public IActionResult DoDelete(int id)
        {
            var eventToDeleteFound = DatabaseLayer.DeleteEventWithSurveysByEventId(id);
            if (!eventToDeleteFound)
            {
                return new StatusCodeResult(503);
            }

            return RedirectToAction("Index");
        }

        public IActionResult GetQrCode(int id)
        {
            var url = $"http://{Request.Host.Host}:{Request.Host.Port}/Survey/Show/{id}";

            var data = QRHelper.GetQrImageDataForUrl(url, ImageFormat.Png);

            return File(data, "image/png", $"QRCode_{id}.png");
        }
    }
}