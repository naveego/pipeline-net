using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naveego.Pipeline.Logging
{
    public class LoggerFactory
    {

        public static ILogger NewFileLogger()
        {
            try
            {
                var output = System.IO.File.OpenWrite("publisher.log");
                return new StreamLogger(output);
            }
            catch { }

            return NullLogger.Instance;
        }

        public static ILogger NewDataFlowLogger(string tenant, string dataFlowUrl)
        {
            if (
                string.IsNullOrWhiteSpace(dataFlowUrl) == false &&
                string.IsNullOrWhiteSpace(tenant) == false
            )
            {
                return new DataFlowLogger(tenant, dataFlowUrl);
            }

            return NullLogger.Instance;
        }

        public static ILogger NewConsoleLogger()
        {
            return new ConsoleLogger();
        }

    }
}
