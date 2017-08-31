using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naveego.Pipeline
{
    public class ShapeDefinition
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public string[] Keys { get; set; }

        public IList<PropertyDefinition> Properties { get; set; }
        
    }
}
