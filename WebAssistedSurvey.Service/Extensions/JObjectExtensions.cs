using Newtonsoft.Json.Linq;

namespace WebAssistedSurvey.Service.Extensions
{
    public static class JObjectExtensions
    {
        public static T GetValue<T>(this JObject jObject, string name)
        {
            return (T)jObject.SelectToken(name).ToObject(typeof(T));
        }
    }
}