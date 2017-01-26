using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Integration.WebApi;
using HackerGamesHub.Azure.Api.Controllers;
using HackerGamesHub.Services;

namespace HackerGamesHub.Azure.Api.Infrastructure
{
    public class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers();
            builder.RegisterType<ImageController>();
            builder.RegisterType<ImageService>().As<IImageService>().SingleInstance();

            return builder.Build();
        }
    }
}