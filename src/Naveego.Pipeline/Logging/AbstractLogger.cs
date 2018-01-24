using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naveego.Pipeline.Logging
{
    public abstract class AbstractLogger : ILogger
    {
        public string Name { get; set; }

        public void LogDebug(string message)
        {
            LogMessage(LogLevel.DEBUG, message);
        }

        public void LogDebugFormat(string message, params object[] args)
        {
            LogMessage(LogLevel.DEBUG, string.Format(message, args));
        }

        public void LogError(string message, Exception ex = null)
        {
            LogMessage(LogLevel.ERROR, message, ex);
        }

        public void LogErrorFormat(string message, params object[] args)
        {
            LogMessage(LogLevel.ERROR, string.Format(message, args));
        }

        public void LogErrorFormat(Exception ex, string message, params object[] args)
        {
            LogMessage(LogLevel.ERROR, string.Format(message, args), ex);
        }

        public void LogInfo(string message)
        {
            LogMessage(LogLevel.INFO, message);
        }

        public void LogInfoFormat(string message, params object[] args)
        {
            LogMessage(LogLevel.INFO, string.Format(message, args));
        }

        public void LogWarn(string message, Exception ex = null)
        {
            LogMessage(LogLevel.WARN, message, ex);
        }

        public void LogWarnFormat(string message, params object[] args)
        {
            LogMessage(LogLevel.WARN, string.Format(message, args));
        }

        public void LogWarnFormat(Exception ex, string message, params object[] args)
        {
            LogMessage(LogLevel.WARN, string.Format(message, args), ex);
        }

        public abstract void LogMessage(LogLevel level, string message, Exception ex = null);

        protected string PrepareMessage(LogLevel level, string message)
        {
            return string.Format("[{0}] {1} {2}", DateTime.Now, level.Label, message);
        }
    }
}
