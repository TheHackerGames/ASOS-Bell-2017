using System;
using System.Linq;
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
        private readonly IFaceService faceService;

        public ImageController(IImageService imageService, IFaceService faceService)
        {
            this.imageService = imageService;
            this.faceService = faceService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> SaveImage()
        {
            if (!Request.Content.Headers.ContentLength.HasValue ||
                Request.Content.Headers.ContentLength.Value == 0)
            {
                return BadRequest();
            }
            var image = await Request.Content.ReadAsByteArrayAsync();

            var imageId = await imageService.SaveImage(image);

            var location = BuildUri(imageId);

            Task.Run(() => faceService.Identify(location));
            return Created(location, image);
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

        [HttpGet]
        [Route("all")]
        public async Task<IHttpActionResult> GetAll()
        {
            var ids = await imageService.GetAllImageIds();

            var allImageUris = ids.Select(BuildUri);

            return Ok(allImageUris);
        }
    }
}