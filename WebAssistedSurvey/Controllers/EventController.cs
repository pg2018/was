using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using WebAssistedSurvey.Models;

namespace WebAssistedSurvey.Controllers
{
    public class EventController : Controller
    {
        public IActionResult Index()
        {
            var context = new SurveyContext();

            var events = context.Events.ToList();

            return View(events);
        }

        public IActionResult New()
        {
            Event newEvent = new Event
            {
                Surveys = new List<Survey>(),
                StartDateTime = DateTime.Now
            };

            return View(newEvent);
        }

        public IActionResult Add(Event @event)
        {
            if (!TryValidateModel(@event))
            {
                return View("New", @event);
            }

            if (!@event.IsMultidays)
            {
                @event.EndDateTime = @event.StartDateTime.AddHours(3);
            }

            var context = new SurveyContext();

            context.Events.Add(@event);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Show(int id)
        {
            var context = new SurveyContext();

            var surveyEvent = context.Events.FirstOrDefault(e => e.EventID == id);
            if (surveyEvent == null)
            {
                return NotFound();
            }

            var surveys = context.Surveys.Where(s => s.EventID == surveyEvent.EventID);
            if (surveys.Any())
            {
                surveyEvent.Surveys = new List<Survey>(surveys);
            }

            return View(surveyEvent);
        }

        public IActionResult GetSurveysAsCsv(int id)
        {
            var context = new SurveyContext();

            var surveyEvent = context.Events.FirstOrDefault(e => e.EventID == id);
            if (surveyEvent == null)
            {
                return NotFound();
            }

            var surveys = context.Surveys.Where(s => s.EventID == surveyEvent.EventID);
            if (surveys.Any())
            {
                surveyEvent.Surveys = new List<Survey>(surveys);
            }
            else
            {
                TempData["CsvMessage"] = $"Für die Veranstaltung \"{surveyEvent.Title}\" wurden keine Umfragen zum exportieren gefunden.";
                return View("Index", context.Events.ToList());
            }

            var data = CsvExporter.GetCsvData(surveyEvent);
            return File(data, "text/csv", $"Umfrage_Antworten_{id}.csv");
        }

        public IActionResult ShowSurvey(int id)
        {
            var context = new SurveyContext();

            var survey = context.Surveys.FirstOrDefault(e => e.SurveyID == id);
            if (survey == null)
            {
                return NotFound();
            }

            return View(survey);
        }

        public IActionResult DeleteSurvey(int id)
        {
            var context = new SurveyContext();

            var survey = context.Surveys.FirstOrDefault(e => e.SurveyID == id);
            if (survey == null)
            {
                return NotFound();
            }

            return View(survey);
        }

        public IActionResult Delete(int id)
        {
            var context = new SurveyContext();

            var eventToDelete = context.Events.FirstOrDefault(e => e.EventID == id);
            if (eventToDelete == null)
            {
                return NotFound();
            }

            return View(eventToDelete);
        }

        public IActionResult DoDelete(int id)
        {
            var context = new SurveyContext();

            var eventToDelete = context.Events.FirstOrDefault(e => e.EventID == id);
            if (eventToDelete == null)
            {
                return NotFound();
            }

            var surveys = context.Surveys.Where(s => s.EventID == id);
            foreach (var survey in surveys)
            {
                context.Surveys.Remove(survey);
            }

            context.Events.Remove(eventToDelete);

            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult GetQrCode(int id)
        {
            PayloadGenerator.Url url = new PayloadGenerator.Url($"http://{Request.Host.Host}:{Request.Host.Port}/Survey/Show/{id}");

            QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            var qrBitmap = qrCode.GetGraphic(20);

            byte[] data;
            using (var stream = new MemoryStream())
            {
                qrBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                data = stream.ToArray();
            }
            
            return File(data, "image/png", $"QRCode_{id}.png");
        }
    }
}