using System;
using System.ComponentModel.DataAnnotations;

namespace WebAssistedSurvey.Models
{
    public class Survey
    {
        public int SurveyID { get; set; }

        public int EventID { get; set; }

        public virtual Event Event { get; set; }

        [Display(Name = "Erstellungszeitpunkt")]
        public DateTime Created { get; set; }

        [Display(Name = "Name")]
        public string ContactName { get; set; }

        [Display(Name = "Ich möchte über zukünftige Veranstaltungen informiert werden")]
        public bool WantNewsletter { get; set; }

        [Display(Name = "E-Mail Adresse")]
        [DataType(DataType.EmailAddress)]
        public string ContactEmail { get; set; }

        [Display(Name = "Was hat Ihnen gut gefallen?")]
        [DataType(DataType.MultilineText)]
        public string GoodGuy { get; set; }

        [Display(Name = "Was hat Ihnen nicht gefallen?")]
        [DataType(DataType.MultilineText)]
        public string BadGuy { get; set; }

        [Display(Name = "Platz für sonstige Kommentare")]
        [DataType(DataType.MultilineText)]
        public string Feedback { get; set; }

        [Display(Name = "Woher haben Sie von dieser Veranstaltung erfahren?")]
        public string Source { get; set; }
    }
}