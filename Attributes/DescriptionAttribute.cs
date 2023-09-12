using System;

namespace FlipnoteDotNet.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class DescriptionAttribute : Attribute
    {
        public string Text { get; set; }
        public DescriptionAttribute(string text)
        {
            Text = text;
        }
    }
}
