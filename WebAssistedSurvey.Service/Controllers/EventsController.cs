using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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
            using (var context = new DataContext())
            {
                var events = context.WebEvents.ToList();
                return new JsonResult(events);
            }
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
        public void Post([FromBody] WebEvent webEvent)
        {
            using (var context = new DataContext())
            {
                context.WebEvents.Add(webEvent);
                context.SaveChanges();
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var @event = GetWebEventById(id);
            if (@event == null)
            {
                return;
            }

            using (var context = new DataContext())
            {
                foreach (var survey in @event.WebSurveys)
                {
                    context.WebSurveys.Remove(survey);
                }

                context.WebEvents.Remove(@event);

                context.SaveChanges();
            }
        }

        private WebEvent GetWebEventById(int id)
        {
            using (var context = new DataContext())
            {

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
}
