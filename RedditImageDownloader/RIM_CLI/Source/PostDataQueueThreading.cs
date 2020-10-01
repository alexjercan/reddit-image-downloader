using System.Threading;

namespace RIM_CLI
{
    public class PostDataQueueThreading : IPostDataQueue
    {
        private readonly PostDataQueue _postDataQueue;
        private readonly Mutex _mutex;

        public PostDataQueueThreading(string subreddit)
        {
            _postDataQueue = new PostDataQueue(subreddit);
            _mutex = new Mutex();
        }

        public PostData GetPost()
        {
            _mutex.WaitOne();
            var result = _postDataQueue.GetPost();
            _mutex.ReleaseMutex();
            
            return result;
        }
    }
}