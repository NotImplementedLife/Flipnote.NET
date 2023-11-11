using FlipnoteDotNet.Commons.GUI.Menu;
using System;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Forms
{
    partial class MainForm
    {

        [MenuItem]
        private void FileⰀOpen_Project(object sender, EventArgs e)
        {
            if (OpenProjectDialog.ShowDialog() == DialogResult.OK)
            {
                RunNonBlockingUI(() => Service.LoadProject(OpenProjectDialog.FileName));
            }
        }

        [MenuItem]
        private void FileⰀSave_Project(object sender, EventArgs e)
		{
            if (Service.IsProjectPathSet)
			{
				RunNonBlockingUI(() => Service.SaveProject());
            }
			else
			{
                if (SaveProjectDialog.ShowDialog() == DialogResult.OK)
                {
                    RunNonBlockingUI(() => Service.SaveProject(SaveProjectDialog.FileName));
                }
            }           
		}

        [MenuItem]
        private void FileⰀExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        [MenuItem]
		private void EditⰀAdd_new_sequence(object sender, EventArgs e)
		{
			AddNewSequenceButton.IsChecked = true;
			SequenceTracksViewer.StartSequenceCreateMode();
		}

		[MenuItem]
		private void HelpⰀAbout(object sender, EventArgs e)
		{
			MessageBox.Show($"Flipnote.NET {ProjectConstants.Version}");
		}
    }
}
