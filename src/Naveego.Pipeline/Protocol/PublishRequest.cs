using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Naveego.Pipeline.Publishers;

namespace Naveego.Pipeline.Protocol
{
    public class PublishRequest
    {
        
        public string ShapeName { get; set; }

        public string PublishToAddress { get; set; }
    }
}
