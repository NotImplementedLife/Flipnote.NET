using FlipnoteDotNet.Data.Entities;
using FlipnoteDotNet.Model.Entities;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Forms
{
    partial class MainForm
    {
        #region Service
        private void Service_ActionsListChanged(object sender, EventArgs e)
        {
            UndoButton.IsEnabled = Service.CanUndo;
            RedoButton.IsEnabled = Service.CanRedo;
        }

        private void Service_ProjectChanged(object sender, EventArgs e)
        {
            Invoke(() => SequenceTracksViewer.LoadSequences(Service.Project));
        }

        private void Service_SelectedSequenceChanged(object sender, IEntityReference<Sequence> sequence)
        {
            Invoke(() =>
            {
                SequenceTracksViewer.SetSelectedSequence(sequence);
                AddLayerButton.IsEnabled = sequence != null;
                LayersListBox.LoadLayers(sequence);
            });
        }

        private void Service_LayersListChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("LayersListChanged");
            Invoke(() => LayersListBox.LoadLayers(Service.SelectedSequence));
        }
        private void Service_TracksChanged(object sender, EventArgs e)
        {
            Invoke(() => SequenceTracksViewer.LoadSequences(Service.Project));
        }

        #endregion Service

        private void UndoButton_Click(object sender, EventArgs e) => RunNonBlockingUI(Service.Undo);
        private void RedoButton_Click(object sender, EventArgs e) => RunNonBlockingUI(Service.Redo);

        private void AddLayerButton_Click(object sender, EventArgs e)
            => RunNonBlockingUI(() => Service.AddLayerToSelectedSequence(typeof(TestLayer)));


        #region SequenceTracksViewer
        private void SequenceTracksViewer_UserSequenceAdded(object sender, int trackId, int start, int end)
        {
            RunNonBlockingUI(() => Service.AddSequence(trackId, start, end));
        }

        private void SequenceTracksViewer_UserSequenceMoved(object sender, int sId, int trackId, int start, int end)
        {            
            RunNonBlockingUI(() => Service.MoveSequence(sId, trackId, start, end));
        }

        private void SequenceTracksViewer_UserSelectedSequenceChanged(object sender, int sequenceId)
        {
            RunNonBlockingUI(() => Service.SelectSequence(sequenceId));
        }

        private void SequenceTracksViewer_SequenceCreateModeEnded(object sender, EventArgs e)
        {
            AddNewSequenceButton.IsChecked = false;
            //SequenceTracksViewer.Seq
        }
        #endregion SequenceTracksViewer

        private void AddNewSequenceButton_Click(object sender, EventArgs e)
        {
            AddNewSequenceButton.IsChecked = !AddNewSequenceButton.IsChecked;
            if (AddNewSequenceButton.IsChecked)
            {
                SequenceTracksViewer.StartSequenceCreateMode();
            }
            else
            {
                SequenceTracksViewer.EndSequenceCreateMode();
            }
        }

    }
}
