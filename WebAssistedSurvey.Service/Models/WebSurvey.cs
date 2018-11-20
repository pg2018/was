using System;
using System.ComponentModel.DataAnnotations;

namespace WebAssistedSurvey.Service.Models
{
    public class WebSurvey
    {
        public int WebSurveyID { get; set; }

        public int WebEventID { get; set; }

        public DateTime Created { get; set; }

        public string ContactName { get; set; }

        public bool WantNewsletter { get; set; }

        [DataType(DataType.EmailAddress)]
        public string ContactEmail { get; set; }

        [DataType(DataType.MultilineText)]
        public string GoodGuy { get; set; }

        [DataType(DataType.MultilineText)]
        public string BadGuy { get; set; }

        [DataType(DataType.MultilineText)]
        public string Feedback { get; set; }

        public string Source { get; set; }
    }
}