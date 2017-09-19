using System;

namespace Naveego.Pipeline.Protocol
{
    internal class JPropertyAttribute : Attribute
    {
        private string v;

        public JPropertyAttribute(string v)
        {
            this.v = v;
        }
    }
}