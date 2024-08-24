using FlipnoteDotNet.Drawing.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.App
{
    public static class Config
    {
        public static IVisualRenderer VisualRenderer = new GDIVisualRenderer();
        //public static IVisualRenderer VisualRenderer = new CLVisualRenderer();
    }
}
