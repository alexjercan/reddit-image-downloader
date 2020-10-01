using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace RIM_CLI
{
    public class SubredditBuffer
    {
        private const int MaxBufferSize = 100;
        
        private Queue<PostData> _posts = new Queue<PostData>(MaxBufferSize);
        private readonly string _subredditUrl;
        private PostData _lastPost;
        
        public SubredditBuffer(string subreddit)
        {
            _subredditUrl = $"http://www.reddit.com/r/{subreddit}.json?limit={MaxBufferSize}";
        }
        
        public PostData GetPost()
        {
            if (!_posts.TryDequeue(out _lastPost)) _lastPost = GetPostInternal();
            
            return _lastPost;
        }

        private PostData GetPostInternal()
        {
            FetchPosts();
            return _posts.TryDequeue(out var result) ? result : null;
        }

        private void FetchPosts()
        {
            var url = _lastPost != null ? $"{_subredditUrl}&after={_lastPost.Name}" : _subredditUrl;
            var json = DownloadJson<SubredditObject>(url);
            
            _posts.Clear();
            foreach (var post in json.Data.Posts) _posts.Enqueue(post.Data);
        }

        private static T DownloadJson<T>(string url)
        {
            using var webClient = new WebClient();
            var jsonString = webClient.DownloadString(url);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }  
    }
}