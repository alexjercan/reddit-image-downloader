using System;

namespace RIM_CLI
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var subreddit = args[0];
            var imagesCount = Math.Clamp(Convert.ToInt32(args[1]), 0, 100);

            if (args.Length <= 2)
            {
                Rim(subreddit, imagesCount);
            }
            else
            {
                var nrThreads = Convert.ToInt32(args[2]);
                RimThreading(subreddit, imagesCount, nrThreads);
            }
        }

        private static void Rim(string subreddit, int imagesCount) => 
            new Rim(subreddit, imagesCount).Run();

        private static void RimThreading(string subreddit, int imagesCount, int nrThreads) => 
            new RimThreading(subreddit, imagesCount, nrThreads).Run();
    }
}
