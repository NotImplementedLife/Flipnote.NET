using FlipnoteDotNet.Project;
using FlipnoteDotNet.Utils.GUI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FlipnoteDotNet
{
    public partial class MainForm
    {
        internal FlipnoteProjectService ProjectService { get; } = new FlipnoteProjectService();
        internal FlipnoteProject Project => ProjectService.Project;

        private void InitializeProjectService()
        {
            ProjectService.ProjectChanged += ProjectService_ProjectChanged;
        }

        private void ProjectService_ProjectChanged(object sender, EventArgs e)
        {            
            SequenceTrackViewer.SequenceManager = Project.SequenceManager;            
            SequenceTrackViewer.AdjustSurfaceSize();
            SequenceTrackViewer.Invalidate();
            DrawCanvasAt(SequenceTracksEditor.Viewer.TrackSignPosition);

            PropertyEditor.Target = null;
            LayersEditor.Sequence = null;
        }

        public void CreateNewProject()
        {
            if (Project == null || UserPrompt.Accepts("Do you want to discard the previous changes?")) 
                ProjectService.CreateNewProject();
        }

    }
}
