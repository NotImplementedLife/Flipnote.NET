using System;

namespace FlipnoteDotNet.Commons.GUI.Menu
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class MenuItemAttribute : Attribute
    {
        public string Path { get; }

        public MenuItemAttribute(string path = null)
        {
            Path = path;
        }
    }
}
