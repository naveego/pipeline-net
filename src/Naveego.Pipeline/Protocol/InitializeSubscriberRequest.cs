using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Naveego.Pipeline.Subscribers;

namespace Naveego.Pipeline.Protocol
{
    public class InitializeSubscriberRequest
    {
        public Dictionary<string, string> Meta { get; set; }

        public SubscriberInstance SubscriberInstance { get; set; }
    }
}
