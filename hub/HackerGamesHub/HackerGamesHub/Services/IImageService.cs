using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackerGamesHub.Services
{
    public interface IImageService
    {
        Task<string> SaveImage(byte[] imageContent);
        Task<byte[]> GetImage(string imageId);
        Task<IEnumerable<string>> GetAllImageIds();
    }
}