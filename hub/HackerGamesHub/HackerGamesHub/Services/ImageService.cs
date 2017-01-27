using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HackerGamesHub.Services
{
    public class ImageService : IImageService
    {
        private readonly ConcurrentDictionary<string, byte[]> images;

        public ImageService()
        {
            images = new ConcurrentDictionary<string, byte[]> { ["asos"] = ReadAsosImage() };
        }

        private static byte[] ReadAsosImage()
        {
            using (var stream = typeof(ImageService).Assembly.GetManifestResourceStream("HackerGamesHub.asosphoto.jpg"))
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream?.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        public Task<string> SaveImage(byte[] imageContent)
        {
            var imageId = Guid.NewGuid().ToString();
            images[imageId] = imageContent;
            return Task.FromResult(imageId);
        }

        public Task<byte[]> GetImage(string imageId)
        {
            byte[] result;
            if (!images.TryGetValue(imageId, out result))
            {
                result = new byte[0];
            }

            return Task.FromResult(result);
        }

        public Task<IEnumerable<string>> GetAllImageIds()
        {
            IEnumerable<string> ids = images.Keys.ToArray();
            return Task.FromResult(ids);
        }
    }
}
