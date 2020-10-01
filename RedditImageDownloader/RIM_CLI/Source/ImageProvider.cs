namespace RIM_CLI
{
    public class ImageProvider
    {
        private readonly IPostDataQueue _queue;

        public ImageProvider(IPostDataQueue queue) => _queue = queue;

        public Image GetImage()
        {
            while (true)
            {
                var postData = _queue.GetPost();
                if (postData == null) return new Image();

                var imageData = Networking.DownloadData(postData.Url);

                var image = new Image {Data = imageData, Title = postData.Title};
                if (!image.HasJpegHeader) continue;
                return image;
            }
        }
    }
}