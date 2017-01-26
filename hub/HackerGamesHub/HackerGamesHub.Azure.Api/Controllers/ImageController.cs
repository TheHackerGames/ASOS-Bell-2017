using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using HackerGamesHub.Azure.Api.Results;
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
        [Route("")]
        public async Task<IHttpActionResult> SaveImage([FromBody] byte[] image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest();
            }

            var imageId = await imageService.SaveImage(image);
            return Created(BuildUri(imageId), image);
        }

        private Uri BuildUri(string imageId)
        {
            var leftPart = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            var baseUri = new Uri(leftPart);

            return new Uri(baseUri, $"{Routes.ImageRoute}/{imageId}");
        }

        [HttpGet]
        [Route("{imageId}")]
        public async Task<IHttpActionResult> GetImage([FromUri] string imageId)
        {
            var content = await imageService.GetImage(imageId);

            if (content.Length == 0)
            {
                return NotFound();
            }

            return new ImageResult(content, "image/png");
        }
    }
}