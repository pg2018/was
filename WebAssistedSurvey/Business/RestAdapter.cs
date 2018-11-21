using System;
using System.Collections.Generic;
using System.Net.Http;
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
            var result = new List<Event>();

            var response = client.GetAsync($"{baseUrl}events");

            try
            {
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

            }
            catch (AggregateException)
            {
                return null;
            }

            return result;
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

        internal static void AddEvent(Event newEvent, int eventDuration)
        {
            if (!newEvent.IsMultidays)
            {
                newEvent.EndDateTime = newEvent.StartDateTime.AddHours(eventDuration);
            }

            var json = JsonConvert.SerializeObject(newEvent);
            client.PostAsJsonAsync($"{baseUrl}events", json);
        }

        public static void AddSurvey(Survey survey)
        {
            var json = JsonConvert.SerializeObject(survey);

            client.PostAsJsonAsync($"{baseUrl}surveys", json);
        }

        internal static Survey GetSurveyById(int id)
        {
            var response = client.GetAsync($"{baseUrl}surveys/{id}");

            var jsonResult = response.Result.Content.ReadAsStringAsync().Result;

            JObject obj = JsonConvert.DeserializeObject(jsonResult) as JObject;
            if (obj == null)
            {
                return null;
            }

            return GetSurveyFromJsonToken(obj);
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

        private static Survey GetSurveyFromJsonToken(JObject jObject)
        {
            try
            {
                var survey = new Survey
                {
                    BadGuy = JsonParse<string>(jObject, "badGuy"),
                    GoodGuy = JsonParse<string>(jObject, "goodGuy"),
                    ContactEmail = JsonParse<string>(jObject, "contactEmail"),
                    ContactName = JsonParse<string>(jObject, "contactName"),
                    Feedback = JsonParse<string>(jObject, "feedback"),
                    Source = JsonParse<string>(jObject, "source"),
                    SurveyID = JsonParse<int>(jObject, "webSurveyID"),
                    EventID = JsonParse<int>(jObject, "webEventID"),
                    Created = JsonParse<DateTime>(jObject, "created")
                };

                return survey;
            }
            catch
            {
                return null;
            }
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

            foreach (var item in token.SelectToken("webSurveys"))
            {
                var survey = new Survey
                {
                    BadGuy = JsonParse<string>(item, "badGuy"),
                    GoodGuy = JsonParse<string>(item, "goodGuy"),
                    ContactEmail = JsonParse<string>(item, "contactEmail"),
                    ContactName = JsonParse<string>(item, "contactName"),
                    Feedback = JsonParse<string>(item, "feedback"),
                    Source = JsonParse<string>(item, "source"),
                    SurveyID = JsonParse<int>(item, "webSurveyID"),
                    EventID = JsonParse<int>(item, "webEventID"),
                    Created = JsonParse<DateTime>(item, "created")
                };

                result.Add(survey);
            }

            return result;
        }

        private static T JsonParse<T>(JToken jToken, string name)
        {
            var obj = jToken.SelectToken(name).ToObject(typeof(T));

            return (T)obj;
        }
    }
}