using System;
using System.Threading;

namespace RIM_CLI
{
    public class RimThreading
    {
        private const int NrThreads = 4;
        
        private readonly int _imagesCount;
        private readonly ImagesBuffer _images;
        private readonly SubredditBufferThreading _subredditBuffer;
        private readonly string _outputPath;
        
        public RimThreading(int imagesCount, string subreddit)
        {
            _imagesCount = imagesCount;
            _images = new ImagesBuffer(imagesCount);
            _subredditBuffer = new SubredditBufferThreading(subreddit);
            _outputPath = FileBuilder.CreateDirectory($"Images-{subreddit}");
        }
        
        public void RunThreads()
        {
            var threads = new Thread[NrThreads];
            
            var fileThread = new Thread(CreateImageFilesThread);
            for (var i = 0; i < NrThreads; i++) threads[i] = new Thread(GetImagesThread);

            fileThread.Start();
            var imagesPerThread = _imagesCount / NrThreads;
            for (var i = 0; i < NrThreads - 1; i++)
                threads[i].Start(new GetImagesThreadArgument {Buffer = _subredditBuffer, ImagesPerThread = imagesPerThread});
            threads[NrThreads - 1].Start(new GetImagesThreadArgument
                {Buffer = _subredditBuffer, ImagesPerThread = _imagesCount - (NrThreads - 1) * imagesPerThread});

            fileThread.Join();
            for (var i = 0; i < NrThreads; i++) threads[i].Join();
        }

        private void GetImagesThread(object arg)
        {
            var data = (GetImagesThreadArgument) arg;
            var imagesPerThread = data.ImagesPerThread;
            var imageProvider = new ImageProvider(data.Buffer);
            
            for (var i = 0; i < imagesPerThread; i++)
            {
                var image = imageProvider.GetImage();
                if (image.IsEmpty) return;

                _images.Add(image);
            }
        }

        private void CreateImageFilesThread()
        {
            for (var i = 0; i < _imagesCount; i++)
            {
                var image = _images.Get();
                FileBuilder.CreateImageFile(_outputPath, image);
                Console.WriteLine($"Created File \"{image.Title}\"");
            }
        }

        private class GetImagesThreadArgument
        {
            public SubredditBufferThreading Buffer { get; set; }
            public int ImagesPerThread { get; set; }
        }
    }
}