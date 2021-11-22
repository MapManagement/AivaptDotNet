using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AivaptDotNet.AivaptClases.Json
{
    public class Repository
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("html_url")]
        public string URL { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("watchers_count")]
        public int Watchers { get; set; }

        [JsonPropertyName("stargazers_count")]
        public int Stars { get; set; }

        [JsonPropertyName("open_issues_count")]
        public int OpenIssues { get; set; }

        [JsonPropertyName("topics")]
        public List<string> Topics { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }


    }
}