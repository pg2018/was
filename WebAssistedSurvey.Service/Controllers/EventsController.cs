﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http.Extensions;
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

            var allEvents = context.WebEvents.ToList();

            return new JsonResult(allEvents);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = GetWebEventById(id);
            if (item == null)
            {
                return NotFound();
            }

            return new JsonResult(item);
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

            var newEvent = GetEventFromJsonToken(obj);

            var context = new DataContext();
            context.WebEvents.Add(newEvent);
            context.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var context = new DataContext();

            var webEvent = GetWebEventById(id);
            if (webEvent == null)
            {
                return;
            }

            foreach (var webSurvey in webEvent.WebSurveys)
            {
                context.WebSurveys.Remove(webSurvey);
            }

            context.WebEvents.Remove(webEvent);

            context.SaveChanges();
        }

        private WebEvent GetEventFromJsonToken(JObject item)
        {
            try
            {
                var eventItem = new WebEvent
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
                    eventItem.WebEventID = eventId;
                }

                return eventItem;
            }
            catch
            {
                return null;
            }
        }

        private WebEvent GetWebEventById(int id)
        {
            var context = new DataContext();

            var item = context.WebEvents.FirstOrDefault(e => e.WebEventID == id);
            if (item == null)
            {
                return null;
            }

            var surveysForEvent = context.WebSurveys.Where(s => s.WebEventID == id);
            item.WebSurveys = surveysForEvent.Any() ? new List<WebSurvey>(surveysForEvent) : new List<WebSurvey>();

            return item;
        }
    }
}
