using System;

namespace FlipnoteDotNet.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    internal class DisplayNameAttribute : Attribute
    {
        public string Name { get; set; }

        public DisplayNameAttribute(string name)
        {
            Name = name;
        }
    }
}
