using System;
using System.Threading.Tasks;
using System.Web.Http;
using HackerGamesHub.Services;

namespace HackerGamesHub.Azure.Api.Controllers
{

    [RoutePrefix(Routes.ImageRoute)]
    public class ImageController : ApiController
    {
        private readonly IImageService imageService;

        public ImageController(IImageService imageService)
        {
            this.imageService = imageService;
        }

        [HttpPost]
        public async Task<IHttpActionResult> SaveImage([FromBody] byte[] image)
        {
            var imageId = await imageService.SaveImage(image);
            return Created(BuildUri(imageId), image);
        }

        private Uri BuildUri(string imageId)
        {
            return new Uri($"{Routes.ImageRoute}/{imageId}");
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetImage([FromUri] string imageId)
        {
            return Ok(await imageService.GetImage(imageId));
        }
    }
}