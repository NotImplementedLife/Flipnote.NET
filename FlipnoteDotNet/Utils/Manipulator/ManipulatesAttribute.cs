using System;

namespace FlipnoteDotNet.Utils.Manipulator
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ManipulatesAttribute : Attribute
    {
        public Type TargetType { get; }

        public ManipulatesAttribute(Type targetType)
        {
            TargetType = targetType;
        }
    }
}
