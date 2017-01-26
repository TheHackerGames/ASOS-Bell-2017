using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace HackerGamesHub.Services
{
    public class ImageService : IImageService
    {
        private readonly ConcurrentDictionary<string, byte[]> images = new ConcurrentDictionary<string, byte[]>();

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
    }
}
