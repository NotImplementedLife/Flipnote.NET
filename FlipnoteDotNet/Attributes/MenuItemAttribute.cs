using System;

namespace FlipnoteDotNet.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MenuItemAttribute : Attribute
    {
        public Type TargetFormType { get; set; }
        public string MenuPath { get; set; }

        public MenuItemAttribute(Type targetFormType, string menuPath)
        {
            TargetFormType = targetFormType;
            MenuPath = menuPath;
        }
    }
}
