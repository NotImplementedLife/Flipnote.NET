using System;

namespace FlipnoteDotNet.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class ParameterWildcardAttribute : Attribute
    {
        public string Name { get; set; }

        public ParameterWildcardAttribute(string name)
        {
            Name = name;
        }
    }
}
