using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naveego.Pipeline
{
    public class DataPoint
    {
        public string TenantID { get; set; }

        public string Entity { get; set; }

        public string Source { get; set; }

        public DataPointAction Action { get; set; }
        
        public string[] KeyNames { get; set; }
        
        public IDictionary<string, string> Meta { get; set; }
        
        public IDictionary<string, object> Data { get; set; } 
    }
}
