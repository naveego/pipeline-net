using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Naveego.Pipeline.Protocol
{
    public class DiscoverShapesResponse
    {
        [JsonProperty("shapes")]
        public IList<ShapeDefinition> Shapes { get; set; }

    }
}
