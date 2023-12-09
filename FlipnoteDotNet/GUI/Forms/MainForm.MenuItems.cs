using FlipnoteDotNet.Commons;
using FlipnoteDotNet.Commons.GUI.Menu;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Forms
{
    partial class MainForm
    {


        #region Debug

        class StringViewerDebugDialog : Form
        { 
            public StringViewerDebugDialog(string str)
            {
                Controls.Add(new TextBox
                {
                    Multiline = true,
                    Dock = DockStyle.Fill,
                    Text = str,
                    ReadOnly = true,
                    Font = new System.Drawing.Font("Consolas", 8),
                    ScrollBars = ScrollBars.Vertical,
                    ShortcutsEnabled = false,
                    TabStop = false,
                    WordWrap = false,
                    BackColor = Color.Black,
                    ForeColor = Color.White
                });
                StartPosition = FormStartPosition.CenterParent;
                Width = 600;
                Height = 600;
            }
        }

        private static void DebugDisplay(params object[] obs)
        {
            new StringViewerDebugDialog(obs.Select(o => o?.ToString() ?? "(null)").JoinToString(Environment.NewLine)).ShowDialog();
        }

        [MenuItem]
        private void DebugⰀViewSelectedEntity(object sender, EventArgs e)
        {
            DebugDisplay(
                Service.SelectedEntity?.ToStringFull(),
                $"{Environment.NewLine}CurrentTimestamp = {Service.Timestamp}"
                );
        }

        #endregion Debug

        #region File

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

        #endregion File

        #region Edit
        [MenuItem]
		private void EditⰀAdd_new_sequence(object sender, EventArgs e)
		{
			AddNewSequenceButton.IsChecked = true;
			SequenceTracksViewer.StartSequenceCreateMode();
		}
        #endregion Edit


        [MenuItem]
		private void HelpⰀAbout(object sender, EventArgs e)
		{
			MessageBox.Show($"Flipnote.NET {ProjectConstants.Version}");
		}
    }
}
