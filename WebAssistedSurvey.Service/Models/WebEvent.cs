using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAssistedSurvey.Service.Models
{
    public class WebEvent
    {
        public int WebEventID { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime StartDateTime { get; set; }

        public bool IsMultidays { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? EndDateTime { get; set; }

        [Display(Name = "Veranstaltungstitle")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Summery { get; set; }

        public virtual IEnumerable<WebSurvey> WebSurveys { get; set; }
    }
}