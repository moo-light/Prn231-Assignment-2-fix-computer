using System.Text.Json;
using System.Text.Json.Serialization;

namespace FUCarRentingSystem_RazorPage.Utils
{

    public static class MyJsonSerializer
    {
        private static JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() },
            WriteIndented = false
        };
        public static T? Deserialize<T>(this string data)
        {
            return JsonSerializer.Deserialize<T>(data, options);
        }
        public static string Serialize<T>(this T data)
        {
            return JsonSerializer.Serialize(data, options);
        }
    }
}