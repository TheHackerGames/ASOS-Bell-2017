using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using HackerGamesHub.Azure.Api;
using HackerGamesHub.Azure.Api.Infrastructure;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace HackerGamesHub.Azure.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            ApiConfig.Configure(config);

            app.UseCors(CorsOptions.AllowAll);

            app.MapSignalR();

            app.UseWebApi(config);
        }
    }
}