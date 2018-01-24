using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naveego.Pipeline.Logging
{
    public class ConsoleLogger : AbstractLogger
    {
        public override void LogMessage(LogLevel level, string message, Exception ex = null)
        {
            System.Console.WriteLine(string.Format("[{0}] {1} {2}", DateTime.Now, level.Label, message));
            if(ex != null)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }
    }
}
