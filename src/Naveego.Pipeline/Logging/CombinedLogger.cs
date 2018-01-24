using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naveego.Pipeline.Logging
{
    public class CombinedLogger : ILogger
    {
        private ILogger[] _loggers;

        public CombinedLogger(params ILogger[] loggers)
        {
            _loggers = loggers;
        }

        public string Name => "CombinedLogger";

        public void LogDebug(string message)
        {
            ForEachLogger(l => l.LogDebug(message));
        }

        public void LogDebugFormat(string message, params object[] args)
        {
            ForEachLogger(l => l.LogDebugFormat(message, args));
        }

        public void LogError(string message, Exception ex = null)
        {
            ForEachLogger(l => l.LogError(message, ex));
        }

        public void LogErrorFormat(string message, params object[] args)
        {
            ForEachLogger(l => LogErrorFormat(message, args));
        }

        public void LogErrorFormat(Exception ex, string message, params object[] args)
        {
            ForEachLogger(l => LogErrorFormat(ex, message, args));
        }

        public void LogInfo(string message)
        {
            ForEachLogger(l => LogInfoFormat(message));
        }

        public void LogInfoFormat(string message, params object[] args)
        {
            ForEachLogger(l => LogInfoFormat(message, args));
        }

        public void LogWarn(string message, Exception ex = null)
        {
            ForEachLogger(l => LogWarn(message, ex));
        }

        public void LogWarnFormat(string message, params object[] args)
        {
            ForEachLogger(l => LogWarnFormat(message, args));
        }

        public void LogWarnFormat(Exception ex, string message, params object[] args)
        {
            ForEachLogger(l => LogWarnFormat(ex, message, args));
        }

        private void ForEachLogger(Action<ILogger> action)
        {
            foreach(var logger in _loggers)
            {
                action(logger);
            }
        }
    }
}
