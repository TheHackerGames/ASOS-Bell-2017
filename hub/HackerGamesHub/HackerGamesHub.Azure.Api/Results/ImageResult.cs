using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace HackerGamesHub.Azure.Api.Results
{
    public class ImageResult : IHttpActionResult
    {
        private readonly byte[] _image;
        private readonly string _contentType;

        public ImageResult(byte[] image, string contentType)
        {
            _image = image;
            _contentType = contentType;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);

            responseMessage.Content = new ByteArrayContent(_image);
            responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(_contentType);

            return Task.FromResult(responseMessage);
        }
    }
}