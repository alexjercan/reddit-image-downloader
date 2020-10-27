using System;
using System.Net;
using Newtonsoft.Json;

namespace RIM_CLI
{
    public static class Networking
    {
        public static byte[] DownloadData(string url)
        {
            try
            {
                using var webClient = new WebClient();
                return webClient.DownloadData(url);
            }
            catch (Exception)
            {
                Console.WriteLine($"Error downloading image from {url} - skipping");
                return null;
            }
        }

        public static T DownloadJson<T>(string url)
        {
            try
            {
                using var webClient = new WebClient();
                var jsonString = webClient.DownloadString(url);
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception)
            {
                Console.WriteLine($"Error downloading image from {url} - skipping");
                return default;
            }
        }
    }
}