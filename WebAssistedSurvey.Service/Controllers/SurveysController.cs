using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebAssistedSurvey.Service.Extensions;
using WebAssistedSurvey.Service.Models;

namespace WebAssistedSurvey.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveysController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var context = new DataContext();

            var survey = context.WebSurveys.FirstOrDefault(e => e.WebSurveyID == id);
            if (survey == null)
            {
                return NotFound();
            }
            
            return new JsonResult(survey);
        }

        [HttpGet("forEvent/{id}")]
        public IActionResult ForEvent(int id)
        {
            var context = new DataContext();

            var surveys = context.WebSurveys.Where(s => s.WebEventID == id);
            if (!surveys.Any())
            {
                return NotFound();
            }

            return new JsonResult(surveys);
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
            JObject obj = JsonConvert.DeserializeObject(value) as JObject;
            if (obj == null)
            {
                return;
            }

            var survey = GetSurveyFromJsonToken(obj);

            var context = new DataContext();
            context.WebSurveys.Add(survey);
            context.SaveChanges();
        }

        private WebSurvey GetSurveyFromJsonToken(JObject item)
        {
            try
            {
                var webSurvey = new WebSurvey
                {
                    Created = item.GetValue<DateTime>("Created"),
                    BadGuy = item.GetValue<string>("BadGuy"),
                    GoodGuy = item.GetValue<string>("GoodGuy"),
                    ContactName = item.GetValue<string>("ContactName"),
                    ContactEmail = item.GetValue<string>("ContactEmail"),
                    Feedback = item.GetValue<string>("Feedback"),
                    Source = item.GetValue<string>("Source"),
                    WantNewsletter = item.GetValue<bool>("WantNewsletter"),
                    WebEventID = item.GetValue<int>("EventID")
                };
                
                return webSurvey;
            }
            catch
            {
                return null;
            }
        }
    }
}