using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ReactDemo.Domain.Models;

namespace ReactDemo.Infrastructure.Extensions
{
    public static class SessionExtension
    {
        public static void Set<T>(this ISession session, string key, T value) where T : IAggregateRoot
        {
            string jsonString = JsonConvert.SerializeObject(value);
            session.SetString(key, jsonString);
        }

        public static T Get<T>(this ISession session, string key) where T : IAggregateRoot
        {
            var jsonString = session.GetString(key);
            return string.IsNullOrEmpty(jsonString) ? default(T) : JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}