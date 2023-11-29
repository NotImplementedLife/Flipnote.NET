using FlipnoteDotNet.Commons.GUI.Events;
using FlipnoteDotNet.Commons.GUI.Menu;
using FlipnoteDotNet.Data.Entities;
using FlipnoteDotNet.Model.Entities;
using FlipnoteDotNet.Properties;
using FlipnoteDotNet.Service;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Resources;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Forms
{    
    internal partial class MainForm : SplitAreaForm
    {
        [Event(nameof(FlipnoteDotNetService.ActionsListChanged))]
        [Event(nameof(FlipnoteDotNetService.TracksChanged))]
        [Event(nameof(FlipnoteDotNetService.ProjectChanged))]
        [Event(nameof(FlipnoteDotNetService.SelectedSequenceChanged))]
        [Event(nameof(FlipnoteDotNetService.LayersListChanged))]
        [Event(nameof(FlipnoteDotNetService.CurrentFrameChanged))]
        [Event(nameof(FlipnoteDotNetService.SelectedEntityChanged))]
        [Event(nameof(FlipnoteDotNetService.SelectedLayerChanged))]
        [Event(nameof(FlipnoteDotNetService.SelectedEntityPropertyChanged))]
        private readonly FlipnoteDotNetService Service = new FlipnoteDotNetService();

        public MainForm()
        {            
            Icon = Resources.icon;
            Text = "Flipnote.NET";
            MinimumSize = new Size(700, 500);            
            MenuLoader.Load(TopMenu, this);
            CreateLayout();
            EventLoader.AttachAll(this);                        
        }        

        private void CreateLayout()
        {
            var (menuBar, topToolbar, content) = SplitAreaV(WholeAreaId, "30px", $"{TopMenu.Height}px", "1*");

            var (left, right) = SplitAreaH(content, "2*", "1*");
            var (lTop, lBottom) = SplitAreaV(left, "55*", "45*");
            var (rTop, rBottom) = SplitAreaV(right, "40*", "60*");

            AddControl(menuBar, TopMenu, 1, 1, 1, 1);
            AddControl(lTop, VisualComponentsScene, 3, 3, 3, 3);
            AddControl(lBottom, SequenceTracksViewer, 3, 30, 3, 3);
            AddControl(lBottom, AddNewSequenceButton, 3, 3);

            AddControl(topToolbar, UndoButton, 3, 2);
            AddControl(topToolbar, RedoButton, 3 + 24 * 1, 2);

            AddControl(rTop, LayersListBox, 3, 30, 3, 3);

            AddControl(rTop, AddLayerButton, 3, 3);
            AddControl(rTop, RemoveLayerButton, 3 + 27, 3);
            AddControl(rTop, MoveUpLayerButton, 3 + 2*27, 3);
            AddControl(rTop, MoveDownLayerButton, 3 + 3 * 27, 3);

            AddControl(rBottom, PropertyEditor, 3, 3, 3, 3);            

            //AddControl(right, new Button { Text = "myButton" }, 12, 10, 12);
        }

        /// <summary>
        /// Runs an action on a separate thread, user actions are disabled but UI still responds to events
        /// </summary>
        /// <param name="action"></param>
        private void RunNonBlockingUI(Action action)
        {
            bool finished = false;
            Task.Run(() =>
            {
                Exception exception = null;
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    exception = e;
                }
                finally
                {
                    finished = true;
                }
                if (exception != null)
                {
                    Debug.WriteLine(exception.Message);
                    Debug.WriteLine(exception.StackTrace);
                }                    
            });

            Task.Run(async () =>
            {
                await Task.Delay(200);
                if (finished) return;

                Cursor cursor = Cursors.Default;

                Invoke(() =>
                {
                    cursor = Cursor.Current;
                    Enabled = false;
                    Cursor.Current = Cursors.WaitCursor;
                });

                while (!finished)
                {
                    await Task.Delay(50);
                    Invoke(new Action(() => Cursor.Current = Cursors.WaitCursor));
                }

                Invoke(() =>
                {
                    Enabled = true;
                });
                Invoke(new Action(() => Cursor.Current = cursor));
            });
        }

        private void Invoke(Action action) => base.Invoke(action);

        private void BackgroundControlPaint(object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            var pos = GetLocationRelativeToForm(control);
            var dx = pos.X % 16;
            var dy = pos.Y % 16;
            e.Graphics.FillRectangle(GuiConstants.GetWindowBackgroundBrush(dx, dy), new Rectangle(Point.Empty, control.Size));
        }
        private static Point GetLocationRelativeToForm(Control c)
        {
            if (c is Form) return Point.Empty;
            if (c.Parent is Form) return c.Location;
            var result = c.Location;
            result.Offset(GetLocationRelativeToForm(c.Parent));
            return result;
        }

        #region FileDialogs

        private readonly OpenFileDialog OpenProjectDialog = new OpenFileDialog()
        {
            Filter = "Flipnote.NET project (*.fnprj)|*.fnprj",
            Title = "Open Flipnote.NET project..."
        };


        private readonly SaveFileDialog SaveProjectDialog = new SaveFileDialog()
        {
            Filter = "Flipnote.NET project (*.fnprj)|*.fnprj",
            Title = "Save Flipnote.NET project..."
        };

        #endregion

        #region Overrides
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Service.CreateNewProject();
        }

        protected override void Dispose(bool disposing)
        {
            EventLoader.DetachAll(this);
            base.Dispose(disposing);
        }
        #endregion Overrides
    }
}
