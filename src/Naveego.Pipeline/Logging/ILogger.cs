using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naveego.Pipeline.Logging
{
    public interface ILogger
    {
        //string Name { get; }
        void LogDebug(string message);
        void LogDebugFormat(string message, params object[] args);
        void LogInfo(string message);
        void LogInfoFormat(string message, params object[] args);
        void LogWarn(string message, Exception ex = null);
        void LogWarnFormat(string message, params object[] args);
        void LogWarnFormat(Exception ex, string message, params object[] args);
        void LogError(string message, Exception ex = null);
        void LogErrorFormat(string message, params object[] args);
        void LogErrorFormat(Exception ex, string message, params object[] args);
    }
}
