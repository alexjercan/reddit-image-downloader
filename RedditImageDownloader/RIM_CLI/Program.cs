using System;
using System.Collections.Generic;

namespace RIM_CLI
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var subReddit = args[0];
            var maxImages = Math.Clamp(Convert.ToInt32(args[1]), 0, 100);

            var images = ImageProvider.GetImagesFromSubReddit(subReddit, maxImages);

            WriteImagesToFiles(subReddit, images);
        }

        private static void WriteImagesToFiles(string subReddit, Dictionary<string, byte[]> images)
        {
            var outputDirectory = $"Images-{subReddit}";
            var outputPath = FileBuilder.CreateDirectory(outputDirectory);
            FileBuilder.CreateImageFiles(outputPath, images);
        }
    }
}
