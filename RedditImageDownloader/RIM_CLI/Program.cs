using System;
using System.Collections.Generic;
using System.Threading;

namespace RIM_CLI
{
    internal static class Program
    {
        private const int NrThreads = 4;
        private static Thread[] _threads = new Thread[NrThreads];
        private static Mutex _mutex = new Mutex();
        private static List<Image> _images = new List<Image>();
        
        private static void Main(string[] args)
        {
            var subReddit = args[0];
            var imagesCount = Math.Clamp(Convert.ToInt32(args[1]), 0, 100);

            var subredditBuffer = new SubredditBufferThreading(subReddit);

            var outputPath = FileBuilder.CreateDirectory($"Images-{subReddit}");

            for (var i = 0; i < NrThreads; i++) _threads[i] = new Thread(GetImages);

            var imagesPerThread = imagesCount / NrThreads;
            for (var i = 0; i < NrThreads - 1; i++)
                _threads[i].Start(new ThreadArgument {Buffer = subredditBuffer, ImagesPerThread = imagesPerThread});
            _threads[NrThreads - 1].Start(new ThreadArgument {Buffer = subredditBuffer, ImagesPerThread = imagesCount - (NrThreads - 1) * imagesPerThread});

            for (var i = 0; i < NrThreads; i++) _threads[i].Join();

            foreach (var image in _images)
            {
                FileBuilder.CreateImageFile(outputPath, image);
                Console.WriteLine($"Created File \"{image.Title}\"");
            }
        }

        private static void GetImages(object arg)
        {
            var data = (ThreadArgument) arg;
            var imagesPerThread = data.ImagesPerThread;
            var imageProvider = new ImageProvider(data.Buffer);
            
            for (var i = 0; i < imagesPerThread; i++)
            {
                var image = imageProvider.GetImage();
                if (image.IsEmpty) return;

                _mutex.WaitOne();
                _images.Add(image);
                _mutex.ReleaseMutex();
            }
        }

        private class ThreadArgument
        {
            public ISubredditBuffer Buffer { get; set; }
            public int ImagesPerThread { get; set; }
        }
    }
}
