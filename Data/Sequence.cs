using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Utils;
using FlipnoteDotNet.Utils.Temporal;
using PPMLib.Data;
using System.Collections.Generic;
using System.Drawing;

namespace FlipnoteDotNet.Data
{
    public class Sequence : AbstractTransformableTemporalContext
    {
        [Editable]
        [Atemporal]
        public string Name { get; set; } = "";

        [Editable]
        [Atemporal]
        public Color Color { get; set; } = Color.DodgerBlue;

        [Editable]
        public TimeDependentValue<FlipnotePen> Pen { get; }

        [Editable]
        public TimeDependentValue<FlipnotePaperColor> PaperColor { get; }

        public override int StartTimestamp
        {
            get => base.StartTimestamp;
            set
            {
                int delta = value - base.StartTimestamp;
                Layers.ForEach(l => l.StartTimestamp += delta);
                base.StartTimestamp = value;
            }
        }

        public int StartFrame { get => StartTimestamp; set => StartTimestamp = value; }       

        public int EndFrame { get; set; }        

        public Sequence()
        {
            Pen = new TimeDependentValue<FlipnotePen>(this, FlipnotePen.PaperInverse);
            PaperColor = new TimeDependentValue<FlipnotePaperColor>(this, FlipnotePaperColor.White);
            this.Initialize();
        }
       
        public List<ILayer> Layers { get; private set; } = new List<ILayer>();     


        public ILayer AddLayer(ILayer layer)
        {
            Layers.Add(layer);
            return layer;           
        }    
    }
}
