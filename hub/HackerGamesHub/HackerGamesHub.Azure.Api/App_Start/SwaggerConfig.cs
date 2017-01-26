using System;
using System.IO;
using System.Web.Http;
using Swashbuckle.Application;

namespace HackerGamesHub.Azure.Api
{
    public class SwaggerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "HackerGamesHub.Azure.Api");
                
                c.IncludeXmlComments(
                    Path.Combine(
                        AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                        "bin/comments.xml"));
            }).EnableSwaggerUi(cfg => cfg.EnableDiscoveryUrlSelector());
        }
    }
}