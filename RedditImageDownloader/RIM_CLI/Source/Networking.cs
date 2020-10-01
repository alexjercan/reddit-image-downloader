using System.Net;
using Newtonsoft.Json;

namespace RIM_CLI
{
    public static class Networking
    {
        public static byte[] DownloadData(string url)
        {
            using var webClient = new WebClient();
            return webClient.DownloadData(url);
        }

        public static T DownloadJson<T>(string url)
        {
            using var webClient = new WebClient();
            var jsonString = webClient.DownloadString(url);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}