using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Naveego.Pipeline.Publishers;

namespace Naveego.Pipeline.Protocol
{
    public class InitializePublisherRequest
    {
        public Dictionary<string, string> Meta { get; set; }

        public Dictionary<string,object> Settings { get; set; }
    }
}
