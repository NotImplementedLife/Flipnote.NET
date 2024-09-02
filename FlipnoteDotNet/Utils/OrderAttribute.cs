using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Utils
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class OrderAttribute : Attribute
    {
        public readonly int Order;
        public OrderAttribute([CallerLineNumber] int order = 0)
        {
            Order = order;
        }        
    }

}
