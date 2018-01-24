using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naveego.Pipeline.Logging
{
    public class NullLogger : ILogger
    {

        public static readonly NullLogger Instance = new NullLogger();

        public string Name => "NULL";

        public void LogDebug(string message)
        {
        }

        public void LogDebugFormat(string message, params object[] args)
        {
        }

        public void LogError(string message, Exception ex = null)
        {
        }

        public void LogErrorFormat(string message, params object[] args)
        {
        }

        public void LogErrorFormat(Exception ex, string message, params object[] args)
        {
        }

        public void LogInfo(string message)
        {
        }

        public void LogInfoFormat(string message, params object[] args)
        {
        }

        public void LogWarn(string message, Exception ex = null)
        {
        }

        public void LogWarnFormat(string message, params object[] args)
        {
        }

        public void LogWarnFormat(Exception ex, string message, params object[] args)
        {
        }
    }
}
