using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebAssistedSurvey.Models;

namespace WebAssistedSurvey.Business
{
    internal class RestAdapter
    {
        private static HttpClient client = new HttpClient();
        private const string baseUrl = @"http://localhost:5042/api/";

        internal static IList<Event> GetEvents()
        {
            var response = client.GetAsync($"{baseUrl}events");

            var jsonResult = response.Result.Content.ReadAsStringAsync().Result;

            jsonResult = jsonResult.Replace("webEventID", "eventID");
            jsonResult = jsonResult.Replace("webSurveyID", "surveyID");
            var result = JsonConvert.DeserializeObject<IEnumerable<Event>>(jsonResult).ToList();

            return result;
        }

        internal static Event GetEventById(int id)
        {
            var response = client.GetAsync($"{baseUrl}events/{id}");

            var jsonResult = response.Result.Content.ReadAsStringAsync().Result;

            jsonResult = jsonResult.Replace("webEventID", "eventID");
            jsonResult = jsonResult.Replace("webSurveyID", "surveyID");

            Event @event = JsonConvert.DeserializeObject<Event>(jsonResult);

            // better way to get the nested survey objects?
            var objects = JsonConvert.DeserializeObject<JObject>(jsonResult);
            var surveys = objects["webSurveys"].ToObject<List<Survey>>();
            if (surveys.Any())
            {
                @event.Surveys = new List<Survey>(surveys);
            }

            return @event;
        }

        internal static void AddEvent(Event newEvent, int eventDuration)
        {
            if (!newEvent.IsMultidays)
            {
                newEvent.EndDateTime = newEvent.StartDateTime.AddHours(eventDuration);
            }

            var dataAsString = JsonConvert.SerializeObject(newEvent);

            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            client.PostAsync($"{baseUrl}events", content);
        }

        public static void AddSurvey(Survey survey)
        {
            var dataAsString = JsonConvert.SerializeObject(survey);
            dataAsString = dataAsString.Replace("EventID", "WebEventID");

            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            client.PostAsync($"{baseUrl}surveys", content);
        }

        internal static Survey GetSurveyById(int id)
        {
            var response = client.GetAsync($"{baseUrl}surveys/{id}");

            var jsonResult = response.Result.Content.ReadAsStringAsync().Result;

            var survey = JsonConvert.DeserializeObject<Survey>(jsonResult);

            return survey;
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

        internal static void DeleteEventWithSurveysByEventId(int id)
        {
            client.DeleteAsync($"{baseUrl}events/{id}");
        }
    }
}