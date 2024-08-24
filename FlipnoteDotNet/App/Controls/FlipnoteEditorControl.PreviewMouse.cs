using System.Runtime.InteropServices;

namespace FlipnoteDotNet.App.Controls
{
    public partial class FlipnoteEditorControl : IMessageFilter
    {
        [DllImport("user32.dll")]
        public static extern bool IsChild(IntPtr hWndParent, IntPtr hWnd);

        public bool PreFilterMessage(ref Message m)
        {
            // handle mouse events when cursor is over child
            const int WM_MOUSEMOVE = 0x0200;
            const int WM_LBUTTONDOWN = 0x0201;
            const int WM_LBUTTONUP = 0x0202;

            if (IsChild(Handle, m.HWnd))
            {
                switch (m.Msg)
                {
                    case WM_MOUSEMOVE:
                        {
                            var screenPoint = Control.FromHandle(m.HWnd).PointToScreen(new Point(m.LParam.ToInt32()));
                            var pos = this.PointToClient(screenPoint);
                            var e = new MouseEventArgs(Control.MouseButtons, 0, pos.X, pos.Y, 0);
                            OnMouseMove(e);
                            break;
                        }
                    case WM_LBUTTONDOWN:
                        {
                            var screenPoint = Control.FromHandle(m.HWnd).PointToScreen(new Point(m.LParam.ToInt32()));
                            var pos = this.PointToClient(screenPoint);
                            var e = new MouseEventArgs(Control.MouseButtons, 0, pos.X, pos.Y, 0);
                            OnMouseDown(e);
                            break;
                        }
                    case WM_LBUTTONUP:
                        {
                            var screenPoint = Control.FromHandle(m.HWnd).PointToScreen(new Point(m.LParam.ToInt32()));
                            var pos = this.PointToClient(screenPoint);
                            var e = new MouseEventArgs(Control.MouseButtons, 0, pos.X, pos.Y, 0);
                            OnMouseUp(e);
                            break;
                        }
                }
            }
            return false;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Application.AddMessageFilter(this);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            Application.RemoveMessageFilter(this);
            base.OnHandleDestroyed(e);
        }
    }
}
