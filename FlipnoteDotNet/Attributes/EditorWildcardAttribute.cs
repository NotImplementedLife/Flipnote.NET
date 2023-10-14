using System;

namespace FlipnoteDotNet.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class EditorWildcardAttribute : Attribute
    {
        public string Text { get; set; }

        public EditorWildcardAttribute(string text)
        {
            Text = text;
        }
    }
}
