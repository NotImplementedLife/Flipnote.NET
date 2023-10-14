using System;

namespace PPMLib.Attributes
{
    [AttributeUsage(AttributeTargets.All)]  
    public class DisplayAttribute : Attribute
    {
        public string Text { get; set; }

        public DisplayAttribute(string text)
        {
            Text = text;
        }
    }
}
