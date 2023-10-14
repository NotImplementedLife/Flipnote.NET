using FlipnoteDotNet.Data.Layers;
using FlipnoteDotNet.Data;
using FlipnoteDotNet.Utils.Temporal.ValueTransformers;
using PPMLib.Rendering;
using System.Drawing;
using System;

namespace FlipnoteDotNet.Project
{
    internal class FlipnoteProjectService
    {
        private FlipnoteProject _Project;
        public FlipnoteProject Project
        {
            get => _Project;
            set
            {
                _Project = value;
                ProjectChanged?.Invoke(this, new EventArgs());
            }
        }

        public event EventHandler ProjectChanged;

        public FlipnoteProjectService()
        {            
        }


        public void CreateNewProject()
        {
            var proj = FlipnoteProject.CreateNew();
            var sampleSequence = new Sequence(0, 10) { Name = "Animation sequence", Color = Color.DarkOrange };
            var sampleLayer = new StaticImageLayer(new FlipnoteVisualSource(64, 48));
            sampleLayer.X.PutTransformer(new ConstantValueTransformer<int>((256 - 64) / 2), 0);
            sampleLayer.Y.PutTransformer(new ConstantValueTransformer<int>((192 - 48) / 2), 0);
            sampleLayer.UpdateAllTimeDependentValues();
            sampleSequence.AddLayer(sampleLayer);
            proj.SequenceManager.GetTrack(0).AddSequence(sampleSequence);
            Project = proj;                       
        }               

    }
}
