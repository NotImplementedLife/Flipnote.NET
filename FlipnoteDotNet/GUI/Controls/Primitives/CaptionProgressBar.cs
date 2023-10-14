using System.Drawing;
using System.Runtime.InteropServices;
using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using System.Windows.Forms.Design.Behavior;
using System.Globalization;

namespace FlipnoteDotNet.GUI.Controls.Primitives
{
    // https://www.codeproject.com/Articles/1082902/How-to-Paint-on-Top-of-a-ProgressBar-using-Csharp

    [Description("Provides a ProgressBar which displays its Value as text on a faded background."),
    Designer(typeof(CustomProgressBarDesigner)),
    ToolboxBitmap(typeof(ProgressBar))]
    internal class CaptionProgressBar : ProgressBar
    {
        public CaptionProgressBar()
        {
            base.ForeColor = SystemColors.ControlText;            
        }

        private string _Caption;

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), DefaultValue("")]
        public string Caption { get=>_Caption; set { _Caption = value; Invalidate(); } }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public override Font Font { get => base.Font; set => base.Font = value; }
        [DefaultValue(typeof(Color), "ControlText")]
        public override Color ForeColor { get => base.ForeColor; set => base.ForeColor = value; }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Always), Bindable(false)]
        public override string Text => $"{Caption} ({Value}/{Maximum})";

        protected override void WndProc(ref Message m)
        {
            int message = m.Msg;
            if (message == NativeMethods.WM_PAINT)
            {
                WmPaint(ref m);
                return;
            }
            if (message == NativeMethods.WM_PRINTCLIENT)
            {
                WmPrintClient(ref m);
                return;
            }
            base.WndProc(ref m);
        }

        private void WmPrintClient(ref Message m)
        {
            // Retrieve the device context
            IntPtr hDC = m.WParam;
            // Let Windows paint
            base.WndProc(ref m);
            // Custom painting
            PaintPrivate(hDC);
        }        

        private void PaintPrivate(IntPtr device)
        {
            // Create a Graphics object for the device context
            using (Graphics graphics = Graphics.FromHdc(device))
            {                
                TextRenderer.DrawText(graphics, Text, Font, ClientRectangle, ForeColor);
            }
        }

        private void WmPaint(ref Message m)
        {
            // Create a Handle wrapper
            HandleRef myHandle = new HandleRef(this, Handle);
                        
            // Prepare the window for painting and retrieve a device context
            NativeMethods.PAINTSTRUCT pAINTSTRUCT = new NativeMethods.PAINTSTRUCT();
            IntPtr hDC = UnsafeNativeMethods.BeginPaint(myHandle, ref pAINTSTRUCT);

            try
            {                
                m.WParam = hDC;                
                base.WndProc(ref m);
                using (Graphics graphics = Graphics.FromHdc(hDC)) 
                {
                    TextRenderer.DrawText(graphics, Text, Font, ClientRectangle, ForeColor);
                }
            }
            finally
            {                
                UnsafeNativeMethods.EndPaint(myHandle, ref pAINTSTRUCT);
            }
        }
    }

    internal class CustomProgressBarDesigner : System.Windows.Forms.Design.ControlDesigner
    {
        public CustomProgressBarDesigner()
        {
        }

        /* Gets a list of System.Windows.Forms.Design.Behavior.SnapLine 
        objects, representing alignment points for the edited control. */
        public override IList SnapLines
        {
            get
            {
                // Get the SnapLines collection from the base class
                ArrayList snapList = base.SnapLines as ArrayList;

                // Calculate the Baseline for the Font used by the Control and add it to the SnapLines
                int textBaseline = GetBaseline(base.Control, ContentAlignment.MiddleCenter);
                if (textBaseline > 0) snapList.Add(new SnapLine(SnapLineType.Baseline, textBaseline, SnapLinePriority.Medium));

                return snapList;
            }
        }

        private static int GetBaseline(Control ctrl, ContentAlignment alignment)
        {
            int textAscent = 0;
            int textHeight = 0;

            // Retrieve the ClientRect of the Control
            Rectangle clientRect = ctrl.ClientRectangle;

            // Create a Graphics object for the Control
            using (Graphics graphics = ctrl.CreateGraphics())
            {
                // Retrieve the device context Handle
                IntPtr hDC = graphics.GetHdc();

                // Create a wrapper for the Font of the Control
                Font controlFont = ctrl.Font;
                HandleRef tempFontHandle = new HandleRef(controlFont, controlFont.ToHfont());

                try
                {
                    // Create a wrapper for the device context
                    HandleRef deviceContextHandle = new HandleRef(ctrl, hDC);

                    // Select the Font into the device context
                    IntPtr originalFont = SafeNativeMethods.SelectObject(deviceContextHandle, tempFontHandle);

                    // Create a TEXTMETRIC and calculate metrics for the selected Font
                    NativeMethods.TEXTMETRIC tEXTMETRIC = new NativeMethods.TEXTMETRIC();
                    if (SafeNativeMethods.GetTextMetrics(deviceContextHandle, ref tEXTMETRIC) != 0)
                    {
                        textAscent = (tEXTMETRIC.tmAscent + 1);
                        textHeight = tEXTMETRIC.tmHeight;
                    }

                    // Restore original Font
                    HandleRef originalFontHandle = new HandleRef(ctrl, originalFont);
                    SafeNativeMethods.SelectObject(deviceContextHandle, originalFontHandle);
                }
                finally
                {
                    // Cleanup tempFont
                    SafeNativeMethods.DeleteObject(tempFontHandle);

                    // Release device context
                    graphics.ReleaseHdc(hDC);
                }
            }

            // Calculate return value based on the specified alignment; first check top alignment
            if ((alignment & (ContentAlignment.TopLeft | ContentAlignment.TopCenter | ContentAlignment.TopRight)) != 0)
            {
                return (clientRect.Top + textAscent);
            }

            // Check middle alignment
            if ((alignment & (ContentAlignment.MiddleLeft | ContentAlignment.MiddleCenter | ContentAlignment.MiddleRight)) == 0)
            {
                return ((clientRect.Bottom - textHeight) + textAscent);
            }

            // Assume bottom alignment
            return ((int)Math.Round((double)clientRect.Top + (double)clientRect.Height / 2 - (double)textHeight / 2 + (double)textAscent));
        }
    }
}
