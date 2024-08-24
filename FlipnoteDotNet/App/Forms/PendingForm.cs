using System.Diagnostics;

namespace FlipnoteDotNet.App.Forms
{
    public class PendingForm : Form
    {
        int BarHeight = 20;
        int BarPadding = 20;
        int ThumbWidth = 50;
        int ThumbPos = 0;
        System.Windows.Forms.Timer Timer = new System.Windows.Forms.Timer();

        public PendingForm()
        {
            SuspendLayout();
            ClientSize = new Size(300, 50);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            StartPosition = FormStartPosition.CenterParent;
            ResumeLayout(false);
            PerformLayout();

            Text = "Please wait...";

            Timer.Tick += Timer_Tick;
            Timer.Interval = 100;
            Timer.Start();            
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ThumbPos = (ThumbPos + 3) % 100;
            Invalidate();            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int x0 = BarPadding;
            int x1 = ClientSize.Width - BarPadding;
            int y0 = (ClientSize.Height - BarHeight) / 2;
            int w = x1 - x0;
            int h = BarHeight;

            e.Graphics.SetClip(new Rectangle(x0, y0, w, h));
            e.Graphics.Clear(Color.White);
            e.Graphics.FillRectangle(Brushes.DodgerBlue, x0 + ThumbPos * ClientSize.Width / 100 - ThumbWidth / 2, y0, ThumbWidth, BarHeight);
        }

        private const int WS_SYSMENU = 0x80000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style &= ~WS_SYSMENU;
                return cp;
            }
        }        

        public static void ShowIfNecessary(Control context, Action action)
        {
            var task = Task.Run(action);
            var supervisor = Task.Run(async () =>
            {
                await Task.Delay(50);
                if (!task.IsCompleted)
                {
                    PendingForm pendingForm = null;
                    context.Invoke(() => pendingForm = new PendingForm());
                    context.BeginInvoke(() => pendingForm.ShowDialog());
                    task.Wait();
                    context.Invoke(pendingForm.Close);
                }
            });
        }        

    }
}
