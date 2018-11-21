using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebAssistedSurvey.Models;

namespace WebAssistedSurvey.Business
{
    internal class DatabaseLayer
    {
        internal static Event CreateNewEvent()
        {
            Event newEvent = new Event
            {
                Surveys = new List<Survey>(),
                StartDateTime = DateTime.Now
            };

            return newEvent;
        }
        
        internal static Survey GetSurveyById(int id)
        {
            var context = new SurveyContext();

            return context.Surveys.FirstOrDefault(e => e.SurveyID == id);
        }

        internal static bool DeleteEventWithSurveysByEventId(int id)
        {
            var context = new SurveyContext();

            var eventToDelete = RestAdapter.GetEventById(id);
            if (eventToDelete == null)
            {
                return false;
            }

            IList<Survey> surveys = GetSurveysByEventId(id);
            foreach (var survey in surveys)
            {
                context.Surveys.Remove(survey);
            }

            context.Events.Remove(eventToDelete);
            context.SaveChanges();

            return true;
        }

        internal static bool IsValidEventExisting(int id)
        {
            var surveyEvent = RestAdapter.GetEventById(id);

            var now = DateTime.Now;
            if (surveyEvent.StartDateTime.CompareTo(now) == -1 &&
                surveyEvent.EndDateTime.GetValueOrDefault().CompareTo(now) == 1)
            {
                return true;
            }

            return false;
        }

        internal static Survey CreateNewSurvey(int eventId)
        {
            var surveyEvent = RestAdapter.GetEventById(eventId);

            Survey survey = new Survey
            {
                Event = surveyEvent,
                EventID = surveyEvent.EventID,
                Created = DateTime.Now
            };

            return survey;
        }
        
        private static IList<Survey> GetSurveysByEventId(int eventId)
        {
            var context = new SurveyContext();

            return context.Surveys.Where(s => s.EventID == eventId).ToList();
        }
    }
}