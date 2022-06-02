using System.Collections.Generic;

namespace AivaptDotNet.DataClasses
{
    public class HttpRequestParameters
    {
        public string RequestUrl { get; set; }

        public List<string> Headers { get; set; }

        public string UserAgent { get; set; }
    }
}
