using Newtonsoft.Json;

namespace Naveego.Pipeline.Hosting
{
    [JsonObject()]
    internal class JsonRequest
    {
        public JsonRequest()
        {
        }

        [JsonProperty("jsonrpc")]
        public string JsonRpc => "1.0";

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("params")]
        public object Params { get; set; }

        [JsonProperty("id")]
        public object Id { get; set; }
    }
}
