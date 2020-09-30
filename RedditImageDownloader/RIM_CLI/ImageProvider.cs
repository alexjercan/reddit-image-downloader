using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace RIM_CLI
{
    public static class ImageProvider
    {
        public static Dictionary<string, byte[]> GetImagesFromSubReddit(string subReddit, int maxImages)
        {
            var images = new Dictionary<string, byte[]>();
            var url = $"http://www.reddit.com/r/{subReddit}.json";

            using var webClient = new WebClient();
            var json = webClient.DownloadString(url);
            var subRedditObject = JsonConvert.DeserializeObject<SubRedditObject>(json);
            var posts = subRedditObject.Data.Posts;
            
            var i = 0;

            foreach (var post in posts)
            {
                if (i >= maxImages) break;
                var dataUrl = post.Data.Url;
                var dataTitle = post.Data.Title;

                var imageData = webClient.DownloadData(dataUrl);

                if (!HasJpegHeader(imageData)) continue;
                
                Console.WriteLine($"Downloaded image {dataTitle}");
                images.Add(dataTitle, imageData);
                i++;
            }

            return images;
        }

        private static bool HasJpegHeader(byte[] imageData)
        {
            var soi = BitConverter.ToUInt16(imageData, 0);
            var marker = BitConverter.ToUInt16(imageData, 2);

            return soi == 0xd8ff && (marker & 0xe0ff) == 0xe0ff;
        }
    }
}