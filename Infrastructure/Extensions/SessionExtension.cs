using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ReactDemo.Infrastructure.Extensions
{
    public static class SessionExtension
    {
        public static void Set<T>(this ISession session, string key, T value) 
        {
            string jsonString = JsonConvert.SerializeObject(value);
            session.SetString(key, jsonString);
        }

        public static T Get<T>(this ISession session, string key) 
        {
            var jsonString = session.GetString(key);
            return string.IsNullOrEmpty(jsonString) ? default(T) : JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}