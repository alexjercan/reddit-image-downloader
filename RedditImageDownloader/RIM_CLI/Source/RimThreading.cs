using System;
using System.Threading;

namespace RIM_CLI
{
    public class RimThreading
    {
        private readonly int _nrThreads;
        private readonly int _imagesCount;
        private readonly string _outputPath;
        private readonly ImageProvider _imageProvider;
        private readonly ThreadSafeQueue<Image> _images;
        
        public RimThreading(string subreddit, int imagesCount, int nrThreads)
        {
            _nrThreads = nrThreads;
            _imagesCount = imagesCount;
            _images = new ThreadSafeQueue<Image>(imagesCount);
            var postDataQueue = new PostDataQueueThreading(subreddit);
            _imageProvider = new ImageProvider(postDataQueue);
            _outputPath = FileBuilder.CreateDirectory($"Images-{subreddit}");
        }
        
        public void Run()
        {
            var threads = new Thread[_nrThreads];
            
            var fileThread = new Thread(CreateImageFilesThread);
            for (var i = 0; i < _nrThreads; i++) threads[i] = new Thread(GetImagesThread);

            fileThread.Start();
            var imagesPerThread = _imagesCount / _nrThreads;
            for (var i = 0; i < _nrThreads - 1; i++)
                threads[i].Start(new GetImagesThreadArgument {ImagesPerThread = imagesPerThread});
            threads[_nrThreads - 1].Start(new GetImagesThreadArgument
                {ImagesPerThread = _imagesCount - (_nrThreads - 1) * imagesPerThread});

            fileThread.Join();
            for (var i = 0; i < _nrThreads; i++) threads[i].Join();
        }

        private void GetImagesThread(object arg)
        {
            var data = (GetImagesThreadArgument) arg;
            var imagesPerThread = data.ImagesPerThread;
            
            for (var i = 0; i < imagesPerThread; i++)
            {
                var image = _imageProvider.GetImage();
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
            public int ImagesPerThread { get; set; }
        }
    }
}