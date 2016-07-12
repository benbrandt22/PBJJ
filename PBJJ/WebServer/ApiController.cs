using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using PBJJ.Core;

namespace PBJJ.WebServer
{
    [RestController(InstanceCreationType.PerCall)]
    public class ApiController
    {
        [UriFormat("/status")]
        public GetResponse GetApplicationStatus()
        {
            return new GetResponse(GetResponse.ResponseStatus.OK, StatusProvider.GetCurrentStatus());
        }
        
    }
}
