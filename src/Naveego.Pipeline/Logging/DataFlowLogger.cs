using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Naveego.Pipeline.Logging
{
    public class DataFlowLogger : AbstractLogger
    {

        private readonly string _dataFlowUrl;
        private readonly string _tenant;

        public DataFlowLogger(string tenant, string dataFlowUrl)
        {
            _tenant = tenant;
            _dataFlowUrl = dataFlowUrl + "/logs";
        }

        public override void LogMessage(LogLevel level, string message, Exception ex = null)
        {
            using (var wc = new System.Net.WebClient())
            {
                try
                {
                    wc.Headers["Content-Type"] = "application/json";

                    dynamic logMessage = new JObject();
                    logMessage.level = level.Label;
                    logMessage.ts = DateTime.Now.ToString("o");
                    logMessage.message = message;
                    logMessage.job = "pipeline";
                    logMessage.host = System.Environment.MachineName;
                    logMessage.tenant_id = _tenant;

                    wc.UploadData(_dataFlowUrl, Encoding.UTF8.GetBytes(logMessage.ToString()));
                }
                catch { }
            }
        }
    }
}
