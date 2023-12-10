using FlipnoteDotNet.Commons.GUI.Controls;
using FlipnoteDotNet.Commons.GUI.Events;
using FlipnoteDotNet.GUI.Controls;
using FlipnoteDotNet.GUI.PseudoControls;
using FlipnoteDotNet.GUI.VisualComponentsEditor;
using FlipnoteDotNet.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FlipnoteDotNet.GUI.Controls.EntityPropertyEditor;

namespace FlipnoteDotNet.GUI.Forms
{
    partial class MainForm
    {
        [Event(nameof(Paint), nameof(BackgroundControlPaint))]
		private readonly MenuStrip TopMenu = new MenuStrip
		{
			AutoSize = false
		};

        private readonly SplitContainer Workspace = new SplitContainer
        {
            Orientation = Orientation.Vertical
        };

        private readonly VisualComponentsScene VisualComponentsScene = new VisualComponentsScene
        {
            Dock=DockStyle.Fill
        };

		[Event(nameof(Commons.GUI.Controls.SequenceTracksViewer.SequenceCreateModeEnded))]
		[Event(nameof(Commons.GUI.Controls.SequenceTracksViewer.UserSequenceAdded))]		
		[Event(nameof(Commons.GUI.Controls.SequenceTracksViewer.UserSequenceMoved))]		
		[Event(nameof(Commons.GUI.Controls.SequenceTracksViewer.UserSelectedSequenceChanged))]
		[Event(nameof(Commons.GUI.Controls.SequenceTracksViewer.UserCurrentFrameChanged))]
		private readonly SequenceTracksViewer SequenceTracksViewer = new SequenceTracksViewer();

		[Event(nameof(Click))]
		private readonly PseudoImageButton AddNewSequenceButton = new PseudoImageButton(Resources.ic_seq_add)
		{
			Size = new Size(24, 24),
			IsCheckable = true
		};

		[Event(nameof(Click))]
		private readonly PseudoImageButton UndoButton = new PseudoImageButton(Resources.ic_undo)
		{
			Size = new Size(24, 24),			
			IsEnabled = false
		};

		[Event(nameof(Click))]
		private readonly PseudoImageButton RedoButton = new PseudoImageButton(Resources.ic_redo)
		{
			Size = new Size(24, 24),
			IsEnabled = false
		};

        [Event(nameof(GUI.Controls.LayersListBox.UserLayerClick))]
        private readonly LayersListBox LayersListBox = new LayersListBox();

        [Event(nameof(Click))]
        private readonly PseudoImageButton AddLayerButton = new PseudoImageButton(Resources.ic_layer_add)
        {
            Size = new Size(24, 24),
            IsEnabled = false
        };

        //[Event(nameof(Click))]
        private readonly PseudoImageButton RemoveLayerButton = new PseudoImageButton(Resources.ic_layer_remove)
        {
            Size = new Size(24, 24),
            IsEnabled = false
        };

        //[Event(nameof(Click))]
        private readonly PseudoImageButton MoveUpLayerButton = new PseudoImageButton(Resources.ic_layer_move_up)
        {
            Size = new Size(24, 24),
            IsEnabled = false
        };

        //[Event(nameof(Click))]
        private readonly PseudoImageButton MoveDownLayerButton = new PseudoImageButton(Resources.ic_layer_move_down)
        {
            Size = new Size(24, 24),
            IsEnabled = false
        };

        [Event(nameof(EntityPropertyEditor.PropertyValueChanged))]
        private readonly EntityPropertyEditor PropertyEditor = new EntityPropertyEditor();

        private void CreateLayout()
        {
            Workspace.SuspendLayout();            
            Workspace.Panel1.Controls.Add(VisualComponentsScene);
            Workspace.ResumeLayout();


            var (menuBar, topToolbar, content) = SplitAreaV(WholeAreaId, "30px", $"{TopMenu.Height}px", "1*");

            var (left, right) = SplitAreaH(content, "2*", "1*");
            var (lTop, lBottom) = SplitAreaV(left, "55*", "45*");
            var (rTop, rBottom) = SplitAreaV(right, "40*", "60*");

            AddControl(menuBar, TopMenu, 1, 1, 1, 1);
            AddControl(lTop, Workspace, 3, 3, 3, 3);
            AddControl(lBottom, SequenceTracksViewer, 3, 30, 3, 3);
            AddControl(lBottom, AddNewSequenceButton, 3, 3);

            AddControl(topToolbar, UndoButton, 3, 2);
            AddControl(topToolbar, RedoButton, 3 + 24 * 1, 2);

            AddControl(rTop, LayersListBox, 3, 30, 3, 3);

            AddControl(rTop, AddLayerButton, 3, 3);
            AddControl(rTop, RemoveLayerButton, 3 + 27, 3);
            AddControl(rTop, MoveUpLayerButton, 3 + 2 * 27, 3);
            AddControl(rTop, MoveDownLayerButton, 3 + 3 * 27, 3);

            AddControl(rBottom, PropertyEditor, 3, 3, 3, 3);

            //AddControl(right, new Button { Text = "myButton" }, 12, 10, 12);
        }

    }
}
