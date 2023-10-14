using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.GUI.Controls;
using FlipnoteDotNet.GUI.Forms;
using FlipnoteDotNet.Utils.Export;
using System;
using System.IO;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Menu.Items
{
    [MenuItem(typeof(MainForm), "File/Export/DSi JPEG")]
    internal class MainForm_01_File_03_Export_01_JPEG : MenuItem<MainForm>
    {
        public MainForm_01_File_03_Export_01_JPEG(MainForm form) : base(form) { }

        protected override void OnClick(EventArgs e)
        {
            var progressTrackForm = new ProgressTrackForm();
            progressTrackForm.SetCaption("Exporting frames");
            progressTrackForm.MaximumStepsCount = 1000;

            progressTrackForm.DoWork += (tracker) =>
            {
                tracker.Success = false;
                var archive = JPEGFramesExporter.ExportArchive(Form.Project?.SequenceManager, tracker);
                if (archive == null)
                    return;
                tracker.Success = true;
                tracker.Result = archive;                
            };

            progressTrackForm.WorkSucceeded += (tracker) =>
            {                
                var archive = tracker.Result as byte[];
                var sfd = new SaveFileDialog();
                sfd.Filter = "Zip archive (*.zip)|*.zip";
                if (sfd.ShowDialog() == DialogResult.OK)                
                    File.WriteAllBytes(sfd.FileName, archive);                
            };

            progressTrackForm.Run();
        }



    }
}
