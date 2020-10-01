namespace RIM_CLI
{
    public class ImageProvider
    {
        private readonly SubredditBuffer _buffer;

        public ImageProvider(string subreddit)
        {
            _buffer = new SubredditBuffer(subreddit);
        }

        public Image GetImage()
        {
            while (true)
            {
                var postData = _buffer.GetPost();
                if (postData == null) return new Image();

                var imageData = Networking.DownloadData(postData.Url);

                var image = new Image {Data = imageData, Title = postData.Title};
                if (!image.HasJpegHeader) continue;
                return image;
            }
        }
    }
}