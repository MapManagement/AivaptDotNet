using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json;
using AivaptDotNet.DataClasses;
using AivaptDotNet.Helpers.General;
using AivaptDotNet.Services;
using Discord.WebSocket;

namespace AivaptDotNet.Helpers.Modules
{
    public static class DevelopmentHelper
    {
        #region Methods

        #region Public Methods

        public static Commit GetLatestCommit()
        {
            List<string> headersList = new List<string>() { "application/vnd.github.v3+json" };
            var response = HttpRequests.SimpleGetRequest(
                "https://api.github.com/repos/mapmanagement/AivaptDotNet/commits?page=1&per_page=1",
                headersList,
                ".NET Foundation Repository Reporter"
            );

            List<Commit> commitList = JsonSerializer.Deserialize<List<Commit>>(response.Result);

            if (commitList.Count < 1)
                return null;

            Commit commit = commitList[0];

            return commit;
        }

        public static Repository GetGeneralInformation()
        {
            List<string> headersList = new List<string>() { "application/vnd.github.v3+json" };
            var response = HttpRequests.SimpleGetRequest(
                "https://api.github.com/repos/mapmanagement/AivaptDotNet",
                headersList,
                ".NET Foundation Repository Reporter"
            );

            Repository repo = JsonSerializer.Deserialize<Repository>(response.Result);

            return repo;
        }

        #endregion

        #endregion
    }
}