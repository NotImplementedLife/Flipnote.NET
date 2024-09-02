using System;
using System.Linq;
using System.Runtime.CompilerServices;

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
