using FlipnoteDotNet.Attributes;
using System;

namespace FlipnoteDotNet.GUI.Menu.Items
{
    [MenuItem(typeof(MainForm), "File/Export/DSi JPEG")]
    internal class MainForm_01_File_02_Export_01_JPEG : MenuItem<MainForm>
    {
        public MainForm_01_File_02_Export_01_JPEG(MainForm form) : base(form) { }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }



    }
}
