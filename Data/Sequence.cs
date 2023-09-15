using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.GUI.Properties.EditorFields;
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
        public string Name { get; set; } = "";

        [Editable]
        [PropertyEditorControl(typeof(SequenceColorEditor))]
        public Color Color { get; set; } = Color.DodgerBlue;

        [Editable]
        public TimeDependentValue<FlipnotePen> Pen1 { get; }

        [Editable]
        public TimeDependentValue<FlipnotePen> Pen2 { get; }

        [Editable]
        public TimeDependentValue<FlipnotePaperColor> PaperColor { get; }

        public SequenceTrack Track { get; set; }

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
            Pen1 = new TimeDependentValue<FlipnotePen>(this, FlipnotePen.PaperInverse);
            Pen2 = new TimeDependentValue<FlipnotePen>(this, FlipnotePen.Red);
            PaperColor = new TimeDependentValue<FlipnotePaperColor>(this, FlipnotePaperColor.White);
            this.Initialize();
        }

        public Sequence(int startFrame, int endFrame) : this()
        {
            StartFrame = startFrame;
            EndFrame = endFrame;
        }
       
        public List<ILayer> Layers { get; private set; } = new List<ILayer>();     


        public ILayer AddLayer(ILayer layer)
        {
            Layers.Add(layer);
            return layer;           
        }    
    }
}
