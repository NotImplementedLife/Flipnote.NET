using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.Utils.Temporal;
using PPMLib.Data;
using System.Drawing;

namespace FlipnoteDotNet.Rendering
{
    public class LayerRenderingOptions
    {
        public TimeDependentValue<FlipnotePaperColor> PaperColor { get; }
        public TimeDependentValue<FlipnotePen> Pen1 { get; }
        public TimeDependentValue<FlipnotePen> Pen2 { get; }
        
        public Color GetBackgroundColor(int timestamp) => PaperColor.GetValueAt(timestamp).ToColor();
        public Color GetLayer1Color(int timestamp) => Pen1.GetValueAt(timestamp).ToColor(PaperColor);
        public Color GetLayer2Color(int timestamp) => Pen2.GetValueAt(timestamp).ToColor(PaperColor);

        public Brush GetBackgroundBrush(int timestamp) => PaperColor.GetValueAt(timestamp).ToBrush();
        public Brush GetLayer1Brush(int timestamp) => Pen1.GetValueAt(timestamp).ToBrush(PaperColor);
        public Brush GetLayer2Brush(int timestamp) => Pen2.GetValueAt(timestamp).ToBrush(PaperColor);

        public LayerRenderingOptions(TimeDependentValue<FlipnotePaperColor> paperColor, TimeDependentValue<FlipnotePen> pen1, TimeDependentValue<FlipnotePen> pen2)
        {
            PaperColor = paperColor;
            Pen1 = pen1;
            Pen2 = pen2;            
        }
    }
}
