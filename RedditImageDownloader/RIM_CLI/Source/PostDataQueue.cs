using System.Collections.Generic;

namespace RIM_CLI
{
    public class PostDataQueue : IPostDataQueue
    {
        private const int MaxBufferSize = 100;
        
        private readonly Queue<PostData> _posts = new Queue<PostData>(MaxBufferSize);
        private readonly string _subredditUrl;
        private PostData _lastPost;
        
        public PostDataQueue(string subreddit)
        {
            _subredditUrl = $"http://www.reddit.com/r/{subreddit}.json?limit={MaxBufferSize}";
        }
        
        public PostData GetPost()
        {
            if (!_posts.TryDequeue(out var lastPost)) lastPost = GetPostInternal();

            _lastPost = lastPost;
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
            var json = Networking.DownloadJson<SubredditObject>(url);
            
            _posts.Clear();
            foreach (var post in json.Data.Posts) _posts.Enqueue(post.Data);
        }
    }
}