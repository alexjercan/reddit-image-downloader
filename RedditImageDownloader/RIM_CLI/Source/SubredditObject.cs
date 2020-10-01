using System.Collections.Generic;
using Newtonsoft.Json;

namespace RIM_CLI
{
    public class SubredditObject
    {
        [JsonProperty("data")] public SubredditData Data { get; set; }
    }
    
    public class SubredditData
    {
        [JsonProperty("children")] public IList<PostObject> Posts { get; set; }
    }

    public class PostObject
    {
        [JsonProperty("data")] public PostData Data { get; set; }   
    }

    public class PostData
    {
        [JsonProperty("url")] public string Url { get; set; }
        [JsonProperty("title")] public string Title { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
    }
}