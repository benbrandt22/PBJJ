using Devkoes.Restup.WebServer.File;
using Devkoes.Restup.WebServer.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devkoes.Restup.WebServer.Rest;

namespace PBJJ.WebServer
{
    public static class Startup
    {
        public static async Task Start() {
            var httpServer = new HttpServer(80);

            var restRouteHandler = new RestRouteHandler();
            restRouteHandler.RegisterController<ApiController>();
            httpServer.RegisterRoute("api", restRouteHandler);

            httpServer.RegisterRoute(new StaticFileRouteHandler(@"WebContent"));
            
            await httpServer.StartServerAsync();
        }
    }
}
