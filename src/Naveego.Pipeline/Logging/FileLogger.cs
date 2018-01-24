using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Naveego.Pipeline.Logging
{
    public class StreamLogger : AbstractLogger
    {
        private readonly Stream _output;
        private readonly StreamWriter _writer;

        public StreamLogger(Stream output)
        {
            _output = output;
            _writer = new StreamWriter(_output);
        }

        public override void LogMessage(LogLevel level, string message, Exception ex = null)
        {
            _writer.WriteLine(string.Format("[{0}] {1} {2}", DateTime.Now, level.Label, message));
            if (ex != null)
            {
                _writer.WriteLine(ex.ToString());
            }

            _writer.Flush();
            _output.Flush();
        }

    }
}
