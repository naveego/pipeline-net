using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Naveego.Pipeline.Publishers;
using Newtonsoft.Json;

namespace Naveego.Pipeline.Protocol
{
    public class DiscoverPublisherShapesRequest
    {
        [JsonProperty("instance")]
        public PublisherInstance PublisherInstance { get; set; }
    }
}
