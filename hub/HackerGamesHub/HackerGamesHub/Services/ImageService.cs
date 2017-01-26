using System.Collections.Concurrent;

namespace HackerGamesHub.Services
{
    public class ImageService : IImageService
    {
        private readonly ConcurrentDictionary<string, byte[]> images = new ConcurrentDictionary<string, byte[]>();

        public void SaveImage(string imageId, byte[] imageContent)
        {
            images[imageId] = imageContent;
        }

        public byte[] GetImage(string imageId)
        {
            byte[] result;
            if (!images.TryGetValue(imageId, out result))
            {
                result = new byte[0];
            }

            return result;
        }
    }
}
