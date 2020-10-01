using System;

namespace RIM_CLI
{
    public class Rim
    {
        private readonly int _imagesCount;
        private readonly string _outputPath;
        private readonly ImageProvider _imageProvider;
        
        public Rim(string subreddit, int imagesCount)
        {
            _imagesCount = imagesCount;
            var postDataQueue = new PostDataQueue(subreddit);
            _imageProvider = new ImageProvider(postDataQueue);
            _outputPath = FileBuilder.CreateDirectory($"Images-{subreddit}");
        }

        public void Run()
        {
            for (var i = 0; i < _imagesCount; i++)
            {
                var image = _imageProvider.GetImage();
                FileBuilder.CreateImageFile(_outputPath, image);
                Console.WriteLine($"Created File \"{image.Title}\"");
            }
        }
    }
}