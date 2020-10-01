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
            var url = $"http://www.reddit.com/r/{subReddit}.json?limit={maxImages}";

            using var webClient = new WebClient();
            var json = webClient.DownloadString(url);
            var subRedditObject = JsonConvert.DeserializeObject<SubredditObject>(json);
            var posts = subRedditObject.Data.Posts;

            for (var index = 0; index < posts.Count; index++)
            {
                var post = posts[index];
                var dataUrl = post.Data.Url;
                var dataTitle = post.Data.Title;
                
                if (images.ContainsKey(dataTitle)) continue;

                var imageData = webClient.DownloadData(dataUrl);

                if (!HasJpegHeader(imageData)) continue;

                Console.WriteLine($"Downloaded image {dataTitle}");
                images.Add(dataTitle, imageData);
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