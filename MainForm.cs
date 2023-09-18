using FlipnoteDotNet.Constants;
using FlipnoteDotNet.Data;
using FlipnoteDotNet.Data.Layers;
using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.GUI;
using FlipnoteDotNet.GUI.Canvas.Components;
using FlipnoteDotNet.GUI.Controls;
using FlipnoteDotNet.GUI.Properties;
using FlipnoteDotNet.GUI.Tracks;
using FlipnoteDotNet.Rendering;
using FlipnoteDotNet.Utils.Temporal;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FlipnoteDotNet
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();                       

            LeftContainer.Panel1.EnableDoubleBuffer();
            LeftContainer.Panel2.EnableDoubleBuffer();
            RightContainer.Panel1.EnableDoubleBuffer();
            RightContainer.Panel2.EnableDoubleBuffer();

            RightContainer.Panel1.Paint += BottomLine_Paint;
            RightContainer.Panel2.Paint += TopLine_Paint;

            RightContainer.DisableSelectable();

            ToolStrip.Renderer = new ToolStripEmptyRenderer();

            SequenceTracksEditor.Viewer.Zoom = int.MaxValue;
            SequenceTracksEditor.ToolStrip.Paint += BackgroundControlPaint;

            PropertyEditor.KeyFramesEditor = KeyFramesEditor;
        }


        SequenceManager SequenceManager = new SequenceManager(5);
        SequenceTracksViewer SequenceTrackViewer => SequenceTracksEditor.Viewer;        

        private void MainForm_Load(object sender, EventArgs e)
        {
            Canvas.CanvasViewLocation = Point.Empty;

            SequenceTrackViewer.SequenceManager = SequenceManager;

            SequenceTrackViewer.SequenceManager.GetTrack(0).AddSequence(new Sequence(0, 7) { Name = "Tralalaala" });
            SequenceTrackViewer.SequenceManager.GetTrack(1).AddSequence(new Sequence(1, 5) { Name = "This Sequence", Color = Color.DarkBlue });

            var s = new Sequence(2, 8) { Name = "otherSeq" };
            s.AddLayer(new StaticImageLayer(new Data.Drawing.FlipnoteVisualSource(5, 5)) { DisplayName = "xaxa" });
            s.AddLayer(new StaticImageLayer(new Data.Drawing.FlipnoteVisualSource(5, 5)));

            SequenceTrackViewer.SequenceManager.GetTrack(2).AddSequence(s);

            SequenceTrackViewer.AdjustSurfaceSize();
            SequenceTrackViewer.Invalidate();

            DrawCanvasAt(0);
            Canvas.CanvasComponents.Add(new BitmapComponent(new Bitmap(@"C:\Users\NotImpLife\Desktop\test.jpg"), 50, 70, 100, 100));
        }

        private void BackgroundControlPaint(object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            var pos = GetLocationRelativeToForm(control);

            var dx = pos.X % 16;
            var dy = pos.Y % 16;

            e.Graphics.FillRectangle(Constants.Brushes.GetWindowBackgroundBrush(dx, dy), new Rectangle(Point.Empty, control.Size));
        }

        private Point GetLocationRelativeToForm(Control c)
        {
            if (c == this) return Point.Empty;
            if (c.Parent == this) return c.Location;
            var result = c.Location;
            result.Offset(GetLocationRelativeToForm(c.Parent));
            return result;
        }

        private void PropertiesExpander_Resize(object sender, EventArgs e)
        {
            PropertyEditor.Width = PropertiesExpander.Width;            
        }

        private void KeyframesExpander_Resize(object sender, EventArgs e)
        {
            KeyFramesEditor.Width = PropertiesExpander.Width;            
        }

        private void SequenceTracksEditor_SelectedElementChanged(object sender, Sequence e)
        {
            if (e != null) 
                e.CurrentTimestamp = SequenceTracksEditor.Viewer.TrackSignPosition;

            LayersEditor.ClearSelection();
            PropertyEditor.Target = LayersEditor.Sequence = e;
        }

        private void PropertyEditor_ObjectPropertyChanged(object sender, System.Reflection.PropertyInfo e)
        {
            Debug.WriteLine("Property changed");
            UpdateSelectedElementLabel();
            if (PropertyEditor.Target is Sequence) 
            {                                
                SequenceTracksEditor.Viewer.InvalidateSurface();
                DrawCanvasAt(SequenceTracksEditor.Viewer.TrackSignPosition);
                return;
            }          
            if(PropertyEditor.Target is ILayer)
            {                
                LayersEditor.LayersListBox.Invalidate();
                DrawCanvasAt(SequenceTracksEditor.Viewer.TrackSignPosition);
                return;
            }
        }

        private void PropertyEditor_TargetChanged(object sender, EventArgs e)
        {
            //PropertiesExpander.IsExpanded = false;
            PropertiesExpander.IsExpanded = PropertyEditor.Target != null;
            Debug.WriteLine($"Target chnaged :{PropertyEditor.Target?.ToString() ?? "null"}");
            UpdateSelectedElementLabel();

            KeyFramesEditor.ClearEditors();
        }

        private void UpdateSelectedElementLabel()
        {
            SelectedElementLabel.Text = PropertyEditor.Target != null
                ? PropertyEditor.Target.GetType().Name : "";
        }

        private ILayer SelectedLayerElement = null;

        private void LayersEditor_SelectionChanged(object sender, GUI.Layers.LayersEditor.SelectionChangedEventArgs e)
        {
            if (e.Layer != null)
                e.Layer.CurrentTimestamp = SequenceTracksEditor.Viewer.TrackSignPosition;
            PropertyEditor.Target = SelectedLayerElement = e.Layer;
        }

        LayerComponentsManager LayerComponentsManager = new LayerComponentsManager();

        private void SequenceTracksEditor_CurrentFrameChanged(object sender, EventArgs e)
        {            
            DrawCanvasAt(SequenceTracksEditor.Viewer.TrackSignPosition);            

            var editedElement = PropertyEditor.Target as ITemporalContext;
            if (editedElement == null)
                return;
            editedElement.CurrentTimestamp = SequenceTracksEditor.Viewer.TrackSignPosition;
        }

        private void DrawCanvasAt(int frame)
        {
            Debug.WriteLine($"LAYERS AT {frame}");
            LayerComponentsManager.UpdateTimestamp(frame);            

            var newComps = LayerComponentsManager.GetFromSequenceManager(SequenceManager).ToList();
            Canvas.CanvasComponents.Clear();
            newComps.ForEach(Canvas.CanvasComponents.Add);
            
            Canvas.CanvasComponents.ForEach(_ =>
                {
                    if(_ is ILayerCanvasComponent lcc)
                        lcc.Layer.UserUpdate += Layer_UserUpdate;
                });

        
            Canvas.Invalidate();            
        }

        private void Layer_UserUpdate(object sender, EventArgs e)
        {            
            if (sender != PropertyEditor.Target) return;            
            PropertyEditor.ReloadValues();                      
        }

        private void PropertyEditor_KeyFramesButtonClick(object sender, EventArgs e)
        {
            KeyframesExpander.IsExpanded = true;
        }

        private void BottomLine_Paint(object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            e.Graphics.DrawLine(Colors.FlipnoteThemeMainColor.GetPen(), 0, control.Height - 2, control.Width, control.Height - 2);
        }

        private void TopLine_Paint(object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            e.Graphics.DrawLine(Colors.FlipnoteThemeMainColor.GetPen(), 0, 2, control.Width, 2);
        }

        private void Canvas_SelectionChanged(object sender, EventArgs e)
        {
            if (Canvas.CanvasComponents.SelectedComponentsCount != 1)
            {

            }
            else
            {
                var selection = Canvas.CanvasComponents.SelectedComponents.First() as ILayerCanvasComponent;
                if (selection == null)
                    return;
                selection.Layer.CurrentTimestamp = SequenceTracksEditor.Viewer.TrackSignPosition;
                PropertyEditor.Target = selection.Layer;
            }
        }
    }
}
