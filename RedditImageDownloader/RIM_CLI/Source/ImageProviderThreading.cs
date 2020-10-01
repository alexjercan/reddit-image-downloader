using System.Collections.Generic;
using System.Threading;

namespace RIM_CLI
{
    public class ImageProviderThreading
    {
        private const int NrThreads = 4;
        
        private Thread[] _threads = new Thread[NrThreads];
        private Mutex _mutex = new Mutex();
        private List<Image> _images = new List<Image>();

        private int _imagesCount;
        private SubredditBufferThreading _subredditBuffer;
        
        public ImageProviderThreading(int imagesCount, SubredditBufferThreading subredditBuffer)
        {
            _imagesCount = imagesCount;
            _subredditBuffer = subredditBuffer;
        }
        
        public IEnumerable<Image> GetImages()
        {
            for (var i = 0; i < NrThreads; i++) _threads[i] = new Thread(GetImagesThread);

            var imagesPerThread = _imagesCount / NrThreads;
            for (var i = 0; i < NrThreads - 1; i++)
                _threads[i].Start(new ThreadArgument {Buffer = _subredditBuffer, ImagesPerThread = imagesPerThread});
            _threads[NrThreads - 1].Start(new ThreadArgument
                {Buffer = _subredditBuffer, ImagesPerThread = _imagesCount - (NrThreads - 1) * imagesPerThread});

            for (var i = 0; i < NrThreads; i++) _threads[i].Join();

            return _images;
        }

        private void GetImagesThread(object arg)
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
            public SubredditBufferThreading Buffer { get; set; }
            public int ImagesPerThread { get; set; }
        }
    }
}