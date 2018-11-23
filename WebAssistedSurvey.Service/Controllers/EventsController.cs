using System;
using System.Collections.Generic;
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
    public class EventsController : ControllerBase
    {
        // GET api/events
        [HttpGet]
        public IActionResult Get()
        {
            var context = new DataContext();

            var events = context.WebEvents.ToList();

            return new JsonResult(events);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var @event = GetWebEventById(id);
            if (@event == null)
            {
                return NotFound();
            }

            return new JsonResult(@event);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
            JObject obj = JsonConvert.DeserializeObject(value) as JObject;
            if (obj == null)
            {
                return;
            }

            var @event = GetEventFromJsonToken(obj);

            var context = new DataContext();
            context.WebEvents.Add(@event);
            context.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var context = new DataContext();

            var @event = GetWebEventById(id);
            if (@event == null)
            {
                return;
            }

            foreach (var survey in @event.WebSurveys)
            {
                context.WebSurveys.Remove(survey);
            }

            context.WebEvents.Remove(@event);

            context.SaveChanges();
        }

        private WebEvent GetEventFromJsonToken(JObject item)
        {
            try
            {
                var @event = new WebEvent
                {
                    StartDateTime = item.GetValue<DateTime>("StartDateTime"),
                    IsMultidays = item.GetValue<bool>("IsMultidays"),
                    EndDateTime = item.GetValue<DateTime>("EndDateTime"),
                    Title = item.GetValue<string>("Title"),
                    Summery = item.GetValue<string>("Summery")
                };

                var eventId = item.GetValue<int>("EventID");
                if (eventId != 0)
                {
                    @event.WebEventID = eventId;
                }

                return @event;
            }
            catch
            {
                return null;
            }
        }

        private WebEvent GetWebEventById(int id)
        {
            var context = new DataContext();

            var @event = context.WebEvents.FirstOrDefault(e => e.WebEventID == id);
            if (@event == null)
            {
                return null;
            }

            var surveys = context.WebSurveys.Where(s => s.WebEventID == id);
            @event.WebSurveys = surveys.Any() ? new List<WebSurvey>(surveys) : new List<WebSurvey>();

            return @event;
        }
    }
}
