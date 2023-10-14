using System;

namespace FlipnoteDotNet.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class PaintToolAttribute : Attribute
    {
        public string ToolName { get; set; }
        public string IconResourceName { get; set; }
        public Type PaintContextEditor { get; set; }
        public PaintToolAttribute(string toolName, string iconResourceName)
        {
            ToolName = toolName;
            IconResourceName = iconResourceName;
        }
    }
}
