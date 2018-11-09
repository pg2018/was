using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAssistedSurvey.Models
{
    public class Event
    {
        public int EventID { get; set; }

        [Required(ErrorMessage = "{0} muss angegeben werden.")]
        [Display(Name = "Start der Veranstaltung")]
        [DataType(DataType.DateTime)]
        public DateTime StartDateTime { get; set; }

        [Display(Name = "Ist eine mehrtägige Veranstaltung")]
        public bool IsMultidays { get; set; }

        [Display(Name = "Ende der Veranstaltung")]
        [DataType(DataType.DateTime)]
        public DateTime? EndDateTime { get; set; }

        [Required(ErrorMessage = "{0} muss angegeben werden.")]
        [Display(Name = "Veranstaltungstitle")]
        public string Title { get; set; }

        [Required(ErrorMessage = "{0} muss angegeben werden.")]
        [Display(Name = "Kurzbeschreibung der Veranstaltung")]
        [DataType(DataType.MultilineText)]
        public string Summery { get; set; }

        [Display(Name = "Umfragen")]
        public virtual IList<Survey> Surveys { get; set; }
    }
}