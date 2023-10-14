using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.GUI.Controls.Primitives
{
    public interface IToolStripCheckButton
    {
        bool CheckOnClick { get; set; }
        bool Checked { get; set; }
        Image Image { get; set; }
        string ToolTipText { get; set; }
        object Tag { get; set; }
        event EventHandler Click;
    }
}
