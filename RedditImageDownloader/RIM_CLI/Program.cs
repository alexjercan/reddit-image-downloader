using System;

namespace RIM_CLI
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var subReddit = args[0];
            var imagesCount = Math.Clamp(Convert.ToInt32(args[1]), 0, 100);

            var subredditBuffer = new SubredditBuffer(subReddit);
            var imageProvider = new ImageProvider(subredditBuffer);

            var outputPath = FileBuilder.CreateDirectory($"Images-{subReddit}");

            for (var i = 0; i < imagesCount; i++)
            {
                var image = imageProvider.GetImage();
                if (image.IsEmpty) return;
                FileBuilder.CreateImageFile(outputPath, image);
            }
        }
    }
}
