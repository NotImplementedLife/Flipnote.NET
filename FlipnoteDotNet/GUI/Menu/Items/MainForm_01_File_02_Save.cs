using FlipnoteDotNet.Attributes;
using System;
using System.IO;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Menu.Items
{
    [MenuItem(typeof(MainForm), "File/Save")]
    internal class MainForm_01_File_02_Save : MenuItem<MainForm>
    {
        public MainForm_01_File_02_Save(MainForm form) : base(form)
        {
        }

        protected override void OnClick(EventArgs e)
        {
            var path = Form.Project.Path ?? UserChoosePath();
            if (path == null) return;

            File.WriteAllBytes(path, Form.Project.Serialize());            
            Form.Project.Path = path;            
        }

        string UserChoosePath()
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "Flipnote.NET Project (*.flipnet)|*.flipnet";
            return sfd.ShowDialog() == DialogResult.OK ? sfd.FileName : null;

        }
    }
}
