namespace RIM_CLI
{
    public class ImageProvider
    {
        private readonly SubredditBuffer _buffer;

        public ImageProvider(SubredditBuffer buffer)
        {
            _buffer = buffer; 
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