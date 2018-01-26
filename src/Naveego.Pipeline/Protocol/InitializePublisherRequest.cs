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
        private Dictionary<string, string> _meta;

        public Dictionary<string, string> Meta
        {
            get
            {
                if (_meta == null)
                {
                    _meta = new Dictionary<string, string>();
                }

                return _meta;
            }
            set { _meta = value; }
        }

        public Dictionary<string, object> Settings { get; set; }
    }
}
