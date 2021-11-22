using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;

using AivaptDotNet.AivaptClases.Json;

namespace AivaptDotNet.Helpers
{
    static class HttpRequests
    {
        public static async Task<string> SimpleGetRequest(string url, List<string> headers, string userAgent)
        {
            using(HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                foreach(string header in headers)
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(header));
                }
                client.DefaultRequestHeaders.Add("User-Agent", userAgent);

                string repoTask = await client.GetStringAsync(url);
                return repoTask;
            }
        }
    }
}