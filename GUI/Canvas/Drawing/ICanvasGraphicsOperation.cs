using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.GUI.Canvas.Drawing
{
    internal interface ICanvasGraphicsOperation
    {
        void Execute(CanvasGraphicsRenderer renderer);
    }
}
