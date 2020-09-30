using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RIM_CLI
{
    public static class FileBuilder
    {
        public static string CreateDirectory(string directoryName)
        {
            var currentPath = Directory.GetCurrentDirectory();
            
            var directoryPath = Path.Combine(currentPath, directoryName);
            
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            return directoryPath;
        }
        
        public static void CreateImageFiles(string directoryPath, Dictionary<string, byte[]> images)
        {
            foreach (var (key, imageData) in images)
            {
                var fileName = $"{key}.jpg";
                if (!IsFileNameValid(fileName)) RemoveInvalidCharacters(ref fileName);
                
                Console.WriteLine($"Creating File {fileName}");
                
                var filePath = Path.Combine(directoryPath, fileName);
                var fileStream = File.Create(filePath);
                fileStream.Write(imageData);
            }
        }

        private static bool IsFileNameValid(string fileName) => 
            !string.IsNullOrEmpty(fileName) && fileName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;

        private static void RemoveInvalidCharacters(ref string fileName)
        {
            var builder = new StringBuilder(fileName.Length);
            foreach (var symbol in fileName.Where(symbol => !Path.GetInvalidFileNameChars().Contains(symbol)))
                builder.Append(symbol);
            fileName = builder.ToString();
        }
    }
}