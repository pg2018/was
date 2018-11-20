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
        private static HttpClient client = new HttpClient();
        private const string baseUrl = @"http://localhost:5042/api/";

        internal static IList<Event> GetEvents()
        {
            var result = new List<Event>();

            var response = client.GetAsync($"{baseUrl}events");

            var jsonResult = response.Result.Content.ReadAsStringAsync().Result;

            JArray jArray = JsonConvert.DeserializeObject(jsonResult) as JArray;
            if (jArray == null)
            {
                return result;
            }

            foreach (var item in jArray)
            {
                var eventItem = GetEventFromJsonToken(item);
                if (eventItem != null)
                {
                    result.Add(eventItem);
                }
            }

            return result;
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
            var response = client.GetAsync($"{baseUrl}events/{id}");

            var jsonResult = response.Result.Content.ReadAsStringAsync().Result;

            JObject obj = JsonConvert.DeserializeObject(jsonResult) as JObject;
            if (obj == null)
            {
                return null;
            }

            return GetEventFromJsonToken(obj);
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

            var json = JsonConvert.SerializeObject(survey);
            var response = client.PostAsJsonAsync(baseUrl, json);

            //context.Surveys.Add(survey);
            //context.SaveChanges();

            return true;
        }

        private static IList<Survey> GetSurveysByEventId(int eventId)
        {
            var context = new SurveyContext();

            return context.Surveys.Where(s => s.EventID == eventId).ToList();
        }

        private static Event GetEventFromJsonToken(JToken item)
        {
            try
            {
                var eventId = JsonParse<int>(item, "webEventID");
                var startDateTime = JsonParse<DateTime>(item, "startDateTime");
                var isMultidays = JsonParse<bool>(item, "isMultidays");
                var endDateTime = JsonParse<DateTime>(item, "endDateTime");
                var title = JsonParse<string>(item, "title");
                var summery = JsonParse<string>(item, "summery");

                var eventItem = new Event
                {
                    EventID = eventId,
                    StartDateTime = startDateTime,
                    IsMultidays = isMultidays,
                    EndDateTime = endDateTime,
                    Title = title,
                    Summery = summery,
                    Surveys = ParseJsonSurveys(item)
                };
                return eventItem;
            }
            catch
            {
                return null;
            }
        }

        private static IList<Survey> ParseJsonSurveys(JToken token)
        {
            var result = new List<Survey>();

            if (!token.SelectToken("webSurveys").HasValues)
            {
                return result;
            }

            return result;
        }

        private static T JsonParse<T>(JToken jToken, string name)
        {
            var obj = jToken.SelectToken(name).ToObject(typeof(T));

            return (T) obj;

            //return (T)jToken.SelectToken(name).ToObject(typeof(T));
        }
    }
}