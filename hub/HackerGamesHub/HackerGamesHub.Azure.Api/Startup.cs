using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Autofac.Integration.WebApi;
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

            var container = ContainerConfig.Configure();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);

            app.UseWebApi(config);
        }
    }
}