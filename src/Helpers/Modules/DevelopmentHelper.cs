using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AivaptDotNet.DataClasses;
using AivaptDotNet.Helpers.General;

namespace AivaptDotNet.Helpers.Modules
{
    public static class DevelopmentHelper
    {
        #region Methods

        #region Public

        public static Commit GetLatestCommit()
        {
            string url = "https://api.github.com/repos/mapmanagement/AivaptDotNet/commits?page=1&per_page=1";
            var requestParameters = GetRequestParameters(url);

            var response = HttpRequests.SimpleGetRequest(requestParameters);

            List<Commit> commitList = JsonSerializer.Deserialize<List<Commit>>(response.Result);

            if (commitList.Count < 1)
                return null;

            Commit commit = commitList[0];

            return commit;
        }

        public static Repository GetGeneralInformation()
        {
            string url = "https://api.github.com/repos/mapmanagement/AivaptDotNet";
            var requestParameters = GetRequestParameters(url);

            var response = HttpRequests.SimpleGetRequest(requestParameters);

            Repository repo = JsonSerializer.Deserialize<Repository>(response.Result);

            return repo;
        }

        public static async Task<Release> GetLatestReleaseAsync()
        {
            string response;
            string url = "https://api.github.com/repos/mapmanagement/AivaptDotNet/releases/latest";
            var requestParameters = GetRequestParameters(url);
            List<string> headersList = new List<string>() { "application/vnd.github.v3+json" };

            try
            {
                response = await HttpRequests.SimpleGetRequest(requestParameters);
            }
            catch (HttpRequestException)
            {
                return null;
            }

            if (response == null)
                return null;
            
            Release rel = JsonSerializer.Deserialize<Release>(response);

            return rel;
        }

        public static async Task<Issue> GetIssueAsync(int issueId)
        {
            string response;
            string url = $"https://api.github.com/repos/mapmanagement/AivaptDotNet/issues/{issueId}";
            var requestParameters = GetRequestParameters(url);

            try
            {
                response = await HttpRequests.SimpleGetRequest(requestParameters);
            }
            catch (HttpRequestException)
            {
                return null;
            }

            if (response == null)
                return null;
            
            Issue issue = JsonSerializer.Deserialize<Issue>(response);

            return issue;
        }

        #endregion

        #region Private

        private static HttpRequestParameters GetRequestParameters(string url)
        {
            List<string> headersList = new List<string>() { "application/vnd.github.v3+json" };
            string defualtUserAgent = ".NET Foundation Repository Reporter";

            var requestParameters = new HttpRequestParameters()
            {
                RequestUrl = url,
                Headers = headersList,
                UserAgent = defualtUserAgent
            };

            return requestParameters;
        }

        #endregion

        #endregion
    }
}
