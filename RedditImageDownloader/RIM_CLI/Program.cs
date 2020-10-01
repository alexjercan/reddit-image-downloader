using System;
using System.Diagnostics;

namespace RIM_CLI
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var subreddit = args[0];
            var imagesCount = Math.Clamp(Convert.ToInt32(args[1]), 0, 100);

            RimThreading(subreddit, imagesCount);
        }

        private static void PerformanceCheck(string subreddit, int imagesCount)
        {
            var stopwatch = new Stopwatch();
            
            stopwatch.Start();
            RimThreading(subreddit, imagesCount);
            stopwatch.Stop();

            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            
            stopwatch.Reset();
            stopwatch.Start();
            Rim(subreddit, imagesCount);
            stopwatch.Stop();
            
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            
        }

        private static void Rim(string subreddit, int imagesCount)
        {
            var subredditBuffer = new SubredditBuffer(subreddit);
            var imageProvider = new ImageProvider(subredditBuffer);
            
            var outputPath = FileBuilder.CreateDirectory($"Images-{subreddit}");

            for (var i = 0; i < imagesCount; i++)
            {
                var image = imageProvider.GetImage();
                FileBuilder.CreateImageFile(outputPath, image);
                Console.WriteLine($"Created File \"{image.Title}\"");
            }
        }
        
        private static void RimThreading(string subreddit, int imagesCount)
        {
            var subredditBuffer = new SubredditBufferThreading(subreddit);
            var imageProvider = new ImageProviderThreading(imagesCount, subredditBuffer);

            var outputPath = FileBuilder.CreateDirectory($"Images-{subreddit}");

            var images = imageProvider.GetImages();
            foreach (var image in images)
            {
                FileBuilder.CreateImageFile(outputPath, image);
                Console.WriteLine($"Created File \"{image.Title}\"");
            }
        }
    }
}
