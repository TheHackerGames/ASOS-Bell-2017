using System;
using System.Threading.Tasks;

namespace HackerGamesHub.Services
{
    public interface IFaceService
    {
        Task Identify(Uri imageLocation);
    }
}