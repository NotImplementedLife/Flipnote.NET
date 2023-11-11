using FlipnoteDotNet.Commons;
using FlipnoteDotNet.Data.Entities;
using FlipnoteDotNet.GUI;
using FlipnoteDotNet.GUI.Properties.EditorFields;
using PPMLib.Data;
using System.ComponentModel;
using System.Drawing;

namespace FlipnoteDotNet.Model.Entities
{
    public sealed class Sequence : Entity
    {
        public string Name { get; set; }

        public int StartFrame { get; set; }
        public int EndFrame { get; set; }

        [FieldEditorAttribute(typeof(SequenceColorEditor))]
        [DefaultValue(typeof(Color), "DodgerBlue")]
        public Color Color { get; set; }

        [Temporal]
        public FlipnotePaperColor PaperColor { get; set; }

        [Temporal]
        public FlipnotePen Pen1 { get; set; }

        [Temporal]
        public FlipnotePen Pen2 { get; set; }

        [Hidden]
        public EntityList<Layer> Layers { get; set; } = new EntityList<Layer>();
    }
}
