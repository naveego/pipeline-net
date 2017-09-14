using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Naveego.Pipeline
{
    public class ShapeDefinition
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("keys")]
        public string[] Keys { get; set; }
        
        [JsonProperty("properties")]
        public IList<PropertyDefinition> Properties { get; set; }
        
    }
}
