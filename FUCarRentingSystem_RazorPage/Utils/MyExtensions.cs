using System.Net;
using System.Web;

namespace FUCarRentingSystem_RazorPage.Utils
{
    public static class MyExtensions
    {
        public static async Task<T?> GetAsync<T>(this HttpClient client, string path, Dictionary<string, object?>? @params = null)
        {
            T? result = default;
            try
            {
                var uri = new Uri(path).AddQuery(@params);
                var response = await client.GetStringAsync(uri);
                result = response.Deserialize<T>();
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);
            }
            return result;
        }

        public static Uri AddQuery(this Uri uri, string name, object? value)
        {

            var ub = new UriBuilder(uri);
            {
                ub.Query += $"&{name}={HttpUtility.UrlEncode(value?.ToString())}";

            }
            return ub.Uri;
        }
        public static Uri AddQuery(this Uri uri, Dictionary<string, object?>? @params = null)
        {
            if (@params == null) return uri;
            foreach (var data in @params)
            {
                uri = uri.AddQuery(data.Key, data.Value?.ToString() ?? "");
            }
            return uri;
        }
    }
}
