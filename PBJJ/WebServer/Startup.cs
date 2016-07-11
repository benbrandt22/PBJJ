using Devkoes.Restup.WebServer.File;
using Devkoes.Restup.WebServer.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBJJ.WebServer
{
    public static class Startup
    {
        public async static Task Start() {
            var httpServer = new HttpServer(80);

            httpServer.RegisterRoute(new StaticFileRouteHandler(@"WebContent"));
            
            await httpServer.StartServerAsync();
        }
    }
}
