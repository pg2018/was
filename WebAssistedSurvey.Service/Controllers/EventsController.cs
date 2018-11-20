using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebAssistedSurvey.Service.Models;

namespace WebAssistedSurvey.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        // GET api/events
        [HttpGet]
        public IActionResult Get()
        {
            var context = new DataContext();

            var allEvents = context.WebEvents.ToList();

            return new JsonResult(allEvents);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var context = new DataContext();

            var item = context.WebEvents.FirstOrDefault(e => e.WebEventID == id);
            if (item == null)
            {
                return NotFound();
            }

            var surveysForEvent = context.WebSurveys.Where(s => s.WebEventID == id);
            if (surveysForEvent.Any())
            {
                item.WebSurveys = new List<WebSurvey>(surveysForEvent);
            }

            return new JsonResult(item);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
            // Console.Out.WriteLine($"{Context.Request.Scheme}://{Context.Request.Host}{Context.Request.Path}{Context.Request.QueryString}");
            Console.Out.WriteLine($"+++ URL = {Request.GetDisplayUrl()}");

            JObject obj = JsonConvert.DeserializeObject(value) as JObject;
            if (obj == null)
            {
                return;
            }

            var webEventId = JsonParse<int>(obj, "EventId");
            var webSurveyId = JsonParse<int>(obj, "SurveyId");
            var contactName = JsonParse<string>(obj, "ContactName");
            var contactEmail = JsonParse<string>(obj, "ContactEmail");
            var wantNewsletter = JsonParse<bool>(obj, "WantNewsletter");
            var goodGuy = JsonParse<string>(obj, "GoodGuy");
            var badGuy = JsonParse<string>(obj, "BadGuy");
            var feedback = JsonParse<string>(obj, "Feedback");
            var source = JsonParse<string>(obj, "Source");

            WebSurvey webSurvey = new WebSurvey
            {
                WebEventID = webEventId,
                WebSurveyID = webSurveyId,
                ContactName = contactName,
                ContactEmail = contactEmail,
                WantNewsletter = wantNewsletter,
                GoodGuy = goodGuy,
                BadGuy = badGuy,
                Feedback = feedback,
                Source = source
            };

            var context = new DataContext();
            context.WebSurveys.Add(webSurvey);
            context.SaveChanges();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private T JsonParse<T>(JObject jObject, string name)
        {
            return (T)jObject.SelectTokens(name);
        }
    }
}
