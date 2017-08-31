using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Naveego.Pipeline
{
    public class DataPoint
    {
        [JsonProperty("tenant")]
        public string TenantID { get; set; }

        [JsonProperty("entity")]
        public string Entity { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("action")]
        public DataPointAction Action { get; set; }
        
        [JsonProperty("keyNames")]
        public string[] KeyNames { get; set; }
        
        [JsonProperty("meta")]
        public IDictionary<string, string> Meta { get; set; }
        
        [JsonProperty("data")]
        public IDictionary<string, object> Data { get; set; } 
    }
}
