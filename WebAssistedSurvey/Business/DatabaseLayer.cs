using System;
using System.Collections.Generic;
using System.Linq;
using WebAssistedSurvey.Models;

namespace WebAssistedSurvey.Business
{
    internal class DatabaseLayer
    {
        internal static IList<Event> GetEvents()
        {
            var context = new SurveyContext();

            return context.Events.ToList();
        }

        internal static Event CreateNewEvent()
        {
            Event newEvent = new Event
            {
                Surveys = new List<Survey>(),
                StartDateTime = DateTime.Now
            };

            return newEvent;
        }

        internal static void AddEvent(Event @event, int eventDuration)
        {
            if (!@event.IsMultidays)
            {
                @event.EndDateTime = @event.StartDateTime.AddHours(eventDuration);
            }

            var context = new SurveyContext();

            context.Events.Add(@event);
            context.SaveChanges();
        }

        internal static Event GetEventById(int id)
        {
            var context = new SurveyContext();

            var surveyEvent = context.Events.FirstOrDefault(e => e.EventID == id);
            if (surveyEvent == null)
            {
                return null;
            }

            var surveys = GetSurveysByEventId(id);
            if (surveys.Any())
            {
                surveyEvent.Surveys = new List<Survey>(surveys);
            }

            return surveyEvent;
        }

        internal static Survey GetSurveyById(int id)
        {
            var context = new SurveyContext();

            return context.Surveys.FirstOrDefault(e => e.SurveyID == id);
        }

        internal static bool DeleteEventWithSurveysByEventId(int id)
        {
            var context = new SurveyContext();

            var eventToDelete = DatabaseLayer.GetEventById(id);
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
            var surveyEvent = GetEventById(id);

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
            var surveyEvent = GetEventById(eventId);

            Survey survey = new Survey
            {
                Event = surveyEvent,
                EventID = surveyEvent.EventID,
                Created = DateTime.Now
            };

            return survey;
        }

        internal static bool AddSurvey(Survey survey)
        {
            var context = new SurveyContext();

            if (GetEventById(survey.EventID) == null)
            {
                return false;
            }

            context.Surveys.Add(survey);
            context.SaveChanges();

            return true;
        }

        private static IList<Survey> GetSurveysByEventId(int eventId)
        {
            var context = new SurveyContext();

            return context.Surveys.Where(s => s.EventID == eventId).ToList();
        }
    }
}