using System.Collections.Generic;
using Newtonsoft.Json;

namespace RIM_CLI
{
    public class SubRedditObject
    {
        [JsonProperty("data")] public SubRedditData Data { get; set; }
    }
    
    public class SubRedditData
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
    }
    
}