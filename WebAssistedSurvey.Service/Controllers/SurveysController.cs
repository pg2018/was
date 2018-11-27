using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            using (var context = new DataContext())
            {
                var survey = context.WebSurveys.FirstOrDefault(e => e.WebSurveyID == id);
                if (survey == null)
                {
                    return NotFound();
                }

                return new JsonResult(survey);
            }
        }

        [HttpGet("forEvent/{id}")]
        public IActionResult ForEvent(int id)
        {
            using (var context = new DataContext())
            {
                var surveys = context.WebSurveys.Where(s => s.WebEventID == id);
                if (!surveys.Any())
                {
                    return NotFound();
                }

                return new JsonResult(surveys);
            }
        }

        [HttpPost]
        public void Post([FromBody] WebSurvey webSurvey)
        {
            using (var context = new DataContext())
            {
                context.WebSurveys.Add(webSurvey);
                context.SaveChanges();
            }
        }
    }
}