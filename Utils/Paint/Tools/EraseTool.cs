using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Utils.Paint.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Utils.Paint.Tools
{
    [PaintTool("Erase",nameof(Properties.Resources.ic_paint_eraser))]
    internal class EraseTool : IPaintTool
    {
        public IPaintOperation CreateOperation() => new EraseOperation();        
    }
}
