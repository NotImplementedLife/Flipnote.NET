using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class LayerAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public Type CreatorForm { get; set; }

    }
}
