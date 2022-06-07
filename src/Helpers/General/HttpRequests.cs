using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;

using AivaptDotNet.DataClasses;

namespace AivaptDotNet.Helpers.General
{
    static class HttpRequests
    {
        public static async Task<string> SimpleGetRequest(HttpRequestParameters requestParameters)
        {
            using(HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();

                foreach(string header in requestParameters.Headers)
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(header));
                }
                
                client.DefaultRequestHeaders.Add("User-Agent", requestParameters.UserAgent);

                string repoTask = await client.GetStringAsync(requestParameters.RequestUrl);
                return repoTask;
            }
        }
    }
}