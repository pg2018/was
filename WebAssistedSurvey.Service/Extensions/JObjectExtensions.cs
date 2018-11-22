using Newtonsoft.Json.Linq;

namespace WebAssistedSurvey.Service.Extensions
{
    public static class JObjectExtensions
    {
        public static T GetValue<T>(this JObject jObject, string name)
        {
            var token = jObject.SelectToken(name);

            var value = token.ToObject<T>();

            return value;
        }
    }
}