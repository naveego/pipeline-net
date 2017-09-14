using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Naveego.Pipeline.Publishers
{
    public class PublisherInstance
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("icon")]
        public string IconURL { get; set; }

        [JsonProperty("shapes")]
        public IList<ShapeDefinition> Shapes { get; set; }

        [JsonProperty("settings")]
        public IDictionary<string, object> Settings { get; set; }
    }
}
