using System.Threading;

namespace RIM_CLI
{
    public class SubredditBufferThreading : ISubredditBuffer
    {
        private readonly SubredditBuffer _subredditBuffer;
        private Mutex _mutex;

        public SubredditBufferThreading(string subreddit)
        {
            _subredditBuffer = new SubredditBuffer(subreddit);
            _mutex = new Mutex();
        }

        public PostData GetPost()
        {
            _mutex.WaitOne();
            var result = _subredditBuffer.GetPost();
            _mutex.ReleaseMutex();
            
            return result;
        }
    }
}