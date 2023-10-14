using System;

namespace FlipnoteDotNet.Attributes
{
    public class CanvasComponentAttribute : Attribute
    {
        public Type ObjectType { get; set; }

        public CanvasComponentAttribute(Type objectType)
        {
            ObjectType = objectType;
        }
    }
}
