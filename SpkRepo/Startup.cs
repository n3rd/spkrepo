using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin;
using System.Web.Http.Routing;
using System.Net.Http;

namespace SpkRepo
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host.
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.IgnoreRoute("index", "", new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });
            config.MapHttpAttributeRoutes();

            appBuilder.UseWebApi(config);

            appBuilder.UseFileServer(new FileServerOptions()
            {
                RequestPath = new PathString(""),
                FileSystem = new PhysicalFileSystem("public"),
            });
        }
    }
}