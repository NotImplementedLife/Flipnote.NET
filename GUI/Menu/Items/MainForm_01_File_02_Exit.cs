using FlipnoteDotNet.Attributes;
using System;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Menu.Items
{
    [MenuItem(typeof(MainForm), "File/Exit")]
    internal class MainForm_01_File_02_Exit : MenuItem<MainForm>
    {
        public MainForm_01_File_02_Exit(MainForm form) : base(form) { }

        protected override void OnClick(EventArgs e)
        {
            Form.Close();
            Application.Exit();
        }
    }
}
