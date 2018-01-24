using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naveego.Pipeline.Logging
{

    public class LogLevel
    {
        public static LogLevel ERROR = new LogLevel("ERROR", 3);
        public static LogLevel WARN = new LogLevel("WARN", 4);
        public static LogLevel INFO = new LogLevel("INFO", 6);
        public static LogLevel DEBUG = new LogLevel("DEBUG", 7);

        private readonly string _label;
        private readonly int _level;

        public string Label => _label;
        public int Level => _level;

        public LogLevel(string label, int level)
        {
            _label = label;
            _level = level;
        }
    }
}
