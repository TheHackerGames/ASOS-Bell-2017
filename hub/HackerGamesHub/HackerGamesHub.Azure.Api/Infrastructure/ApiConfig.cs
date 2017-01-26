using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Owin;

namespace HackerGamesHub.Azure.Api.Infrastructure
{
    public class ApiConfig
    {
        public static void Configure(HttpConfiguration config)
        {
            SwaggerConfig.Register(config);
            WebApiConfig.Register(config);
        }
    }
}