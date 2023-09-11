using System;
using System.Drawing;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Controls
{
    internal class FSplitContainer : SplitContainer
    {
        public FSplitContainer()
        {
            InitializeComponent();            
        }

        public void InitializeComponent()
        {
            this.SetStyle(ControlStyles.Selectable, false);
            TabStop = false;
        }

        public override bool Focused => false;

        protected override void OnGotFocus(EventArgs e)
        {            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);         
        }
    }
}
