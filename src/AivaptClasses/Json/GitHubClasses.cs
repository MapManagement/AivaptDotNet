using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AivaptDotNet.AivaptClases.Json
{
    ///<summary>
    /// Represents a Git repository on GitHub
    ///</summary>
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

    ///<summary>
    /// Represents a commit of a repository on GitHub
    ///</summary>
    public class Commit
    {
        [JsonPropertyName("html_url")]
        public string URL { get; set; }

        [JsonPropertyName("sha")]
        public string SHA { get; set; }

        [JsonPropertyName("author")]
        public GitHubUser Author { get; set; }

        [JsonPropertyName("commit")]
        public CommitDetails Details { get; set; }
    }

    ///<summary>
    /// Represents a user on GitHub
    ///</summary>
    public class GitHubUser
    {
        [JsonPropertyName("login")]
        public string Login { get; set; }

        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("avater_url")]
        public string AvatarUrl { get; set; }

        [JsonPropertyName("html_url")]
        public string URL {get; set; }

        [JsonPropertyName("repos_url")]
        public string ReposUrl { get; set; }
    }

    ///<summary>
    /// Delivers more detailed information about a Git commit
    ///</summary>
    public class CommitDetails
    {
        [JsonPropertyName("author")]
        public GitUser Author { get; set; }

        [JsonPropertyName("committer")]
        public GitUser Committer { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }

    ///<summary>
    /// Represents a Git user
    ///</summary>
    public class GitUser
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string MailAddress { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
    }

    ///<summary>
    /// Represents an issue of a repository on GitHub
    ///</summary>
    public class Issue
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("htmL_url")]
        public string URL { get; set; }

        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("user")]
        public GitHubUser User { get; set; }

        [JsonPropertyName("author_association")]
        public string AuthorAssociation { get; set; }
    }

    ///<summary>
    /// Represents a release of a repository on GitHub
    ///</summary>
    public class Release
    {
        [JsonPropertyName("html_url")]
        public string URL { get; set; }

        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("tag_name")]
        public string TagName { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [JsonPropertyName("published_at")]
        public DateTime PublishedAt { get; set; }

        [JsonPropertyName("author")]
        public GitHubUser Author { get; set; }
    }
}