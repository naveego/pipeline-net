using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naveego.Pipeline
{
    public static class DictionaryExtensions
    {

        public static string GetValueIfPresent(this IDictionary<string, string> d, string key)
        {
            if (d.ContainsKey(key))
            {
                return d[key];
            }

            return null;
        }

    }
}
