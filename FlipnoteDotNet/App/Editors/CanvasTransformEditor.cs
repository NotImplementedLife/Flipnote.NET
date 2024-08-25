using FlipnoteDotNet.Canvas;
using FlipnoteDotNet.Core.Utils;
using FlipnoteDotNet.Properties;
using FlipnoteDotNet.PropertyEditor.Editors;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms.VisualStyles;

namespace FlipnoteDotNet.App.Editors
{
    internal class CanvasTransformEditor : Editor<CanvasTransform>
    {
        public override int RequestedTextRows => 5;
        private readonly StringFormat StringFormat = StringFormat.GenericTypographic;

        public CanvasTransformEditor(): base(invalidateAfterValueChanged: true) 
        {            
            TabPaint = new[]
            {
                TabPaint_Anchor, TabPaint_Translate, TabPaint_Default, TabPaint_Rot
            };

            TabMouseDown = new[]
            {
                TabMouseDown_Anchor, TabMouseDown_Translate, TabMouse_Default, TabMouseDown_Rot,
            };

            TabMouseMove = new[]
            {
                TabMouseMove_Anchor, TabMouse_Default, TabMouse_Default, TabMouseMove_Rot,
            };

            TabMouseUp = new[]
            {
                TabMouseUp_Anchor, TabMouse_Default, TabMouse_Default, TabMouseUp_Rot,
            };
        }

        int TabW = 20;
        int SelectedTab = 0;        
        int Width, Height;

        #region Overrides
        public override void OnPaint(Graphics g, Font font)
        {
            Width = (int)g.ClipBounds.Width;
            Height = (int)g.ClipBounds.Height;
            var x0 = (int)g.ClipBounds.X;
            var y0 = (int)g.ClipBounds.Y;
            
            int tabH = Height / Icons.Length;
            for (int i = 0; i < Icons.Length; i++) 
            {
                DrawIcon(g, Icons[i], x0, y0 + tabH * i, TabW, tabH, i == SelectedTab);
            }
            g.DrawLine(Pens.Gray, x0 + TabW, y0, x0 + TabW, y0 + Height);
            g.SetClip(new Rectangle(x0 + TabW, y0, Width - TabW, Height));
            TabPaint[SelectedTab](g, font);
        }

        public override void OnMouseDown(MouseButtons buttons, int x, int y)
        {
            base.OnMouseDown(buttons, x, y);
            if (x < TabW)
            {
                int tabH = Height / Icons.Length;
                int tabIndex = y / tabH;
                tabIndex = tabIndex < 0 ? 0 : tabIndex >= Icons.Length ? Icons.Length - 1 : tabIndex;
                SelectTab(tabIndex);
                return;
            }
            else
            {
                TabMouseDown[SelectedTab]?.Invoke(buttons, x, y);
            }
        }

        public override void OnMouseMove(MouseButtons buttons, int x, int y)
        {
            base.OnMouseMove(buttons, x, y);
            TabMouseMove[SelectedTab]?.Invoke(buttons, x, y);
        }

        public override void OnMouseUp(MouseButtons buttons, int x, int y)
        {
            base.OnMouseUp(buttons, x, y);
            TabMouseUp[SelectedTab]?.Invoke(buttons, x, y);
        }
        #endregion

        private void SelectTab(int tabIndex)
        {
            if (SelectedTab == tabIndex) return;
            DisposeControl();
            SelectedTab = tabIndex;
            Invalidate();
        }

        #region Icons
        private void DrawIcon(Graphics g, Bitmap icon, int x, int y, int w, int h, bool selected)
        {
            if(selected)
            {
                g.FillRectangle(Brushes.Gray, x, y, w, h);
            }
            if(w>=icon.Width && h >= icon.Height)
            {
                g.DrawImageUnscaled(icon, x + (w - icon.Width) / 2, y + (h - icon.Height) / 2);
            }
            else
            {
                float scale = Math.Min(1f * w / icon.Width, 1f * Height / icon.Height);
                int nw = (int)(icon.Width * scale);
                int nh = (int)(icon.Height * scale);
                g.DrawImage(icon, x + (w - nw) / 2, y + (h - nh) / 2, nw, nh);
            }
        }

        private readonly Bitmap[] Icons = new[]
        {
            Resources.ic_tr_anchor,
            Resources.ic_tr_translate,
            Resources.ic_tr_scale,
            Resources.ic_tr_rot,
        };
        #endregion

        private readonly Action<Graphics, Font>[] TabPaint;
        private readonly Action<MouseButtons, int, int>[] TabMouseDown;
        private readonly Action<MouseButtons, int, int>[] TabMouseMove;
        private readonly Action<MouseButtons, int, int>[] TabMouseUp;

        #region Tab_Rot
        private void TabPaint_Rot(Graphics g, Font font)
        {
            var bw = g.ClipBounds.Width;
            var bh = g.ClipBounds.Height;
            var L = Math.Min(bw, (RequestedTextRows - 2) * bh / RequestedTextRows);
            var l = L * 6 / 8;
            var x0 = g.ClipBounds.X + L / 8;
            var y0 = g.ClipBounds.Y + L / 8;
            using (var pen = new Pen(Color.Black, 2))
                g.DrawEllipse(pen, x0, y0, l, l);

            var ax = x0 + l / 2 + fValue.Rotation.CosValue * l / 2;
            var ay = y0 + l / 2 + fValue.Rotation.SinValue * l / 2;

            using (var pen = new Pen(Color.Black, 2)) 
            {
                g.DrawLine(pen, x0 + l / 2, y0 + l / 2, ax, ay);                
            }
            var text = $"Rot={fValue.Rotation:F4}";
            g.DrawString(text, font, Brushes.Black, x0, g.ClipBounds.Y + (RequestedTextRows - 2) * bh / RequestedTextRows);
        }

        private bool RotTab_UpdateRot(int x, int y, bool checkBounds, bool capture=false)
        {
            var bw = Width;
            var bh = Height;
            var L = Math.Min(bw - TabW, (RequestedTextRows - 2) * bh / RequestedTextRows);
            var l = L * 6 / 8;
            var x0 = TabW + L / 8;
            var y0 = L / 8;
            int r = l / 2;
            int cx = x0 + r, cy = y0 + r;
            int dx = x - cx, dy = y - cy, dd = dx * dx + dy * dy;
            if(capture)
            {
                UserCaptureTransform();
            }
            if (l > 0 && (!checkBounds || dd < r * r)) 
            {
                var angle = (float)(Math.Atan2(dy, dx) * 180 / Math.PI);                
                UserSetTransform(fValue with { Rotation = angle }, preview: true);                
                return true;
            }
            return false;
        }

        private bool RotTab_MsDown = false;

        private void TabMouseDown_Rot(MouseButtons buttons, int x, int y)
        {
            if (RotTab_MsDown = (buttons == MouseButtons.Left))
            {
                if (!RotTab_UpdateRot(x, y, checkBounds: true, capture: true)) 
                {
                    RotTab_MsDown = false;
                }
            }
        }

        private void TabMouseMove_Rot(MouseButtons buttons, int x, int y)
        {
            if (RotTab_MsDown)
            {
                Debug.WriteLine("Moving?");
                RotTab_UpdateRot(x, y, checkBounds: false);
            }
        }

        private void TabMouseUp_Rot(MouseButtons buttons, int x, int y)
        {
            if(RotTab_MsDown)
            {
                UserSetTransform(fValue, preview: false);
            }
            RotTab_MsDown = false;
        }

        #endregion

        #region Tab_Anchor
        private void TabPaint_Anchor(Graphics g, Font font)
        {
            var bw = g.ClipBounds.Width;
            var bh = g.ClipBounds.Height;
            var L = Math.Min(bw, (RequestedTextRows - 2) * bh / RequestedTextRows);
            var l = L * 6 / 8;
            var x0 = g.ClipBounds.X + L / 8;
            var y0 = g.ClipBounds.Y + L / 8;
            using (var pen = new Pen(Color.Black, 2))
                g.DrawRectangle(pen, x0, y0, l, l);
            using (var pen = new Pen(Color.FromArgb(64, Color.Black), 2) { DashStyle = DashStyle.Dash })
            {
                g.DrawLine(pen, x0 + l / 2, y0, x0 + l / 2, y0 + l);
                g.DrawLine(pen, x0, y0 + l / 2, x0 + l, y0 + l / 2);
            }
            var ax = x0 + (int)(l * fValue.AnchorX);
            var ay = y0 + (int)(l * fValue.AnchorY);
            g.FillEllipse(Brushes.Black, ax - 2, ay - 2, 4, 4);
            var text = $"X={fValue.AnchorX:F4}\nY={fValue.AnchorY:F4}";
            g.DrawString(text, font, Brushes.Black, x0, g.ClipBounds.Y + (RequestedTextRows - 2) * bh / RequestedTextRows);
        }
        private bool AnchorTab_UpdateAnchors(int x, int y, bool checkBounds)
        {
            var bw = Width;
            var bh = Height;
            var L = Math.Min(bw - TabW, (RequestedTextRows - 2) * bh / RequestedTextRows);
            var l = L * 6 / 8;
            var x0 = TabW + L / 8;
            var y0 = L / 8;
            if (l > 0 && (!checkBounds || new Rectangle(x0, y0, l, l).Contains(x, y))) 
            {
                float ax = (1f * (x - x0) / l).Clamp(0, 1);
                float ay = (1f * (y - y0) / l).Clamp(0, 1);                                
                UserSetTransform(fValue with { AnchorX = ax, AnchorY = ay }, preview: false);
                return true;
            }
            return false;
        }        

        private bool AnchorTab_MsDown = false;        

        private void TabMouseDown_Anchor(MouseButtons buttons, int x, int y)
        {
            if(AnchorTab_MsDown = (buttons == MouseButtons.Left))
            {
                if (!AnchorTab_UpdateAnchors(x, y, checkBounds: true))
                    AnchorTab_MsDown = false;
            }
        }

        private void TabMouseMove_Anchor(MouseButtons buttons, int x, int y)
        {
            if(AnchorTab_MsDown)
            {
                Debug.WriteLine("Moving?");
                AnchorTab_UpdateAnchors(x, y, checkBounds: false);
            }
        }

        private void TabMouseUp_Anchor(MouseButtons buttons, int x, int y)
        {
            AnchorTab_MsDown = false;
        }


        #endregion

        #region Tab_Translate

        private Rectangle TranslateXControlRect;
        private Rectangle TranslateYControlRect;

        private void TabPaint_Translate(Graphics g, Font font)
        {
            int h = (int)(g.ClipBounds.Height / RequestedTextRows);
            int s = (int)g.MeasureString("X:_", font, (int)g.ClipBounds.Width, StringFormat).Width;

            g.DrawString("X:", font, Brushes.Black, g.ClipBounds.X, g.ClipBounds.Y, StringFormat);
            g.DrawString("Y:", font, Brushes.Black, g.ClipBounds.X, g.ClipBounds.Y + h, StringFormat);

            var rect = new Rectangle((int)g.ClipBounds.X + s, (int)g.ClipBounds.Y, (int)g.ClipBounds.Width - s, h);            
            TextBoxRenderer.DrawTextBox(g, rect, fValue.X.ToString(), font, TextBoxState.Normal);
            TranslateXControlRect = new Rectangle(TabW + s, 0, rect.Width, rect.Height);
            rect.Offset(0, h);
            TextBoxRenderer.DrawTextBox(g, rect, fValue.Y.ToString(), font, TextBoxState.Normal);
            TranslateYControlRect = new Rectangle(TabW + s, h, rect.Width, rect.Height);
        }

        private NumericUpDown CreateNumericInput(int x, int y, int w, int h)
        {
            var c = new NumericUpDown
            {
                Minimum = short.MinValue,
                Maximum = short.MaxValue,
                Tag = new IEditor.SummonedControlStats(this, new Rectangle(x, y, w, h))
            };

            return c;
        }

        private void TabMouseDown_Translate(MouseButtons buttons, int x, int y)
        {                        
            if (TranslateXControlRect.Contains(x, y))
            {
                int bx = TranslateXControlRect.X, by = TranslateXControlRect.Y;
                int w = TranslateXControlRect.Width, h = TranslateXControlRect.Height;
                int dw = Width - (w + bx);
                var control = CreateNumericInput(bx, by, -bx, h);
                control.Value = fValue.X;
                control.ValueChanged += (o, e) =>
                {
                    UserCaptureTransform();
                    UserSetTransform(fValue with { X = (int)control.Value }, preview: false);                    
                };
                SummonControl(control);
            }
            else if (TranslateYControlRect.Contains(x, y)) 
            {
                int bx = TranslateYControlRect.X, by = TranslateYControlRect.Y;
                int w = TranslateYControlRect.Width, h = TranslateYControlRect.Height;
                int dw = Width - (w + bx);
                var control = CreateNumericInput(bx, by, -bx, h);
                control.Value = fValue.Y;
                control.ValueChanged += (o, e) =>
                {
                    UserCaptureTransform();
                    UserSetTransform(fValue with { Y = (int)control.Value }, preview: false);                    
                };
                SummonControl(control);
            }
        }

        #endregion

        public override void OnFocusLost()
        {
            DisposeControl();
        }

        private void TabPaint_Default(Graphics g, Font font)
        {
            g.DrawString("Ok", font, Brushes.Black, g.ClipBounds);
        }        

        private void TabMouse_Default(MouseButtons buttons, int x, int y) { }

        private CanvasTransform oldTransform;
        private void UserCaptureTransform()
        {
            oldTransform = fValue;
        }

        private void UserSetTransform(CanvasTransform newTransform, bool preview)
        {            
            Value = newTransform;
            TriggerUserValueChanged(oldTransform, newTransform, preview);
        }
    }
}
