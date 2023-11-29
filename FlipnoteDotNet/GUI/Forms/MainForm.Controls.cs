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

		private readonly VisualComponentsScene VisualComponentsScene = new VisualComponentsScene();

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

    }
}
