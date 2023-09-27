using FlipnoteDotNet.Attributes;
using System;

namespace FlipnoteDotNet.GUI.Menu.Items
{
    [MenuItem(typeof(MainForm), "File/New")]
    internal class MainForm_01_File_01_New : MenuItem<MainForm>
    {
        public MainForm_01_File_01_New(MainForm form) : base(form) { }

        protected override void OnClick(EventArgs e)
        {
            Form.CreateNewProject();
        }
    }  
}
