using FlipnoteDotNet.Data;
using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.GUI;
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
using FlipnoteDotNet.GUI.Layers;
using FlipnoteDotNet.Rendering.Frames;
using System.Threading;
using FlipnoteDotNet.Utils;
using FlipnoteDotNet.GUI.Menu;
using FlipnoteDotNet.Utils.GUI;

namespace FlipnoteDotNet
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            MenuItemsLoader.LoadMenuItems(FormMenu);

            Collections.Of(LeftContainer.Panel1, LeftContainer.Panel2, RightContainer.Panel1, RightContainer.Panel2)
                .ForEach(_ => _.EnableDoubleBuffer());            

            RightContainer.Panel1.Paint += PaintEvents.BottomLine_Paint;
            RightContainer.Panel2.Paint += PaintEvents.TopLine_Paint;

            RightContainer.DisableSelectable();

            ToolStrip.Renderer = new ToolStripEmptyRenderer();

            SequenceTracksEditor.Viewer.Zoom = int.MaxValue;
            SequenceTracksEditor.ToolStrip.Paint += PaintEvents.BackgroundControlPaint;

            PropertyEditor.KeyFramesEditor = KeyFramesEditor;

            ThumbnailsSource.LockWrite();
            Collections.Generate(1000, () => new Bitmap(256, 192)).ForEach(ThumbnailsSource.AddNoLock);
            ThumbnailsSource.UnlockWrite();
            SequenceTracksEditor.Viewer.ThumbnailsSource = ThumbnailsSource;

            Collections.Of<Control>(FormMenu, ToolStrip, MainContainer, LeftContainer.Panel2,
                             LeftContainer, RightContainer.Panel1, RightContainer.Panel2)
                .ForEach(_ => _.Paint += PaintEvents.BackgroundControlPaint);

            InitializeProjectService();
        }
        
        SequenceTracksViewer SequenceTrackViewer => SequenceTracksEditor.Viewer;        

        private void MainForm_Load(object sender, EventArgs e)
        {
            Canvas.ClearComponents();
            Canvas.CanvasViewLocation = Point.Empty;
            CreateNewProject();
            ThumbnailsRendererBgWorker.RunWorkerAsync();
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
            if (PropertyEditor.Target is ILayer) 
            {                
                LayersEditor.LayersListBox.Invalidate();
                DrawCanvasAt(SequenceTracksEditor.Viewer.TrackSignPosition);
                return;
            }
        }

        private void PropertyEditor_TargetChanged(object sender, EventArgs e)
        {            
            PropertiesExpander.IsExpanded = PropertyEditor.Target != null;
            Debug.WriteLine($"Target chnaged :{PropertyEditor.Target?.ToString() ?? "null"}");
            UpdateSelectedElementLabel();
            
            if (PropertyEditor.Target is ILayer layer)
            {
                Debug.WriteLine("Target changed!");
                LayersEditor.LayersListBox.SelectLayer(layer);
                var component = Canvas.CanvasComponents.Where(c => c is ILayerCanvasComponent lcc && lcc.Layer == layer).FirstOrDefault();
                Canvas.SelectSingle(component);
            }

            KeyFramesEditor.ClearEditors();
        }

        private void UpdateSelectedElementLabel()
        {
            SelectedElementLabel.Text = PropertyEditor.Target != null
                ? PropertyEditor.Target.GetType().Name : "";
        }        

        private void LayersEditor_SelectionChanged(object sender, LayersEditor.SelectionChangedEventArgs e)
        {
            if (e.Layer != null)
                e.Layer.CurrentTimestamp = SequenceTracksEditor.Viewer.TrackSignPosition;
            PropertyEditor.Target = e.Layer;
        }

        private readonly LayerComponentsManager LayerComponentsManager = new LayerComponentsManager();

        private void SequenceTracksEditor_CurrentFrameChanged(object sender, EventArgs e)
        {
            DrawCanvasAt(SequenceTracksEditor.Viewer.TrackSignPosition);
            if (!(PropertyEditor.Target is ITemporalContext editedElement)) 
                return;
            editedElement.CurrentTimestamp = SequenceTracksEditor.Viewer.TrackSignPosition;
        }

        private void DrawCanvasAt(int frame)
        {
            LayerComponentsManager.UpdateTimestamp(frame);

            Canvas.ClearComponents();
            
            if (Project != null)
            {
                LayerComponentsManager
                    .GetFromSequenceManager(Project.SequenceManager)
                    .ForEach(Canvas.CanvasComponents.Add);

                Canvas.CanvasComponents.ForEach(_ =>
                {
                    if (_ is ILayerCanvasComponent lcc)
                        lcc.Layer.UserUpdate += Layer_UserUpdate;
                });
            }
        
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

        private void Canvas_SelectionChanged(object sender, EventArgs e)
        {            
            if (Canvas.CanvasComponents.SelectedComponentsCount == 1)
            {
                var selection = Canvas.CanvasComponents.SelectedComponents.First() as ILayerCanvasComponent;
                if (selection == null)
                    return;
                selection.Layer.CurrentTimestamp = SequenceTracksEditor.Viewer.TrackSignPosition;
                PropertyEditor.Target = selection.Layer;
            }                      
        }

        private void LayersEditor_LayersListChanged(object sender, EventArgs e)
        {
            DrawCanvasAt(SequenceTracksEditor.Viewer.TrackSignPosition);
        }

        private void ThumbnailsRendererBgWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {            
            while(true)
            {
                if (Project == null) continue;
                int i = 0;
                foreach (var frame in FlipnoteFramesRenderer.CreateFrames(Project.SequenceManager)) 
                {
                    ThumbnailsSource.LockWrite();
                    ThumbnailsSource.GetNoLock(i)?.Dispose();
                    ThumbnailsSource.SetNoLock(i, frame.ToBitmap());
                    ThumbnailsSource.UnlockWrite();
                    if (i % 50 == 0)
                        SequenceTracksEditor.Viewer.InvalidateSurface();
                    i++;
                    Thread.Sleep(50);
                }
                Thread.Sleep(2000);                
            }
        }

        private readonly ConcurrentList<Bitmap> ThumbnailsSource = new ConcurrentList<Bitmap>();
    }
}
