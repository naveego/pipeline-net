﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naveego.Pipeline.Protocol
{
    public class ReceiveDataPointRequest
    {
        public ShapeDefinition Shape { get; set; }

        public DataPoint DataPoint { get; set; }
    }
}
