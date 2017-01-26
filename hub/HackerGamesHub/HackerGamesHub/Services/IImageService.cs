namespace HackerGamesHub.Services
{
    public interface IImageService
    {
        void SaveImage(string imageId, byte[] imageContent);
        byte[] GetImage(string imageId);
    }
}