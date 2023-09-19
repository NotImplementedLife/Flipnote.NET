using static FlipnoteDotNet.Constants;
using FlipnoteDotNet.Data;
using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.GUI.Canvas;
using FlipnoteDotNet.GUI.Canvas.Components;
using PPMLib.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace FlipnoteDotNet.Rendering
{
    internal class LayerComponentsManager
    {
        private Dictionary<ILayer, ILayerCanvasComponent> Components = new Dictionary<ILayer, ILayerCanvasComponent>();

        public int Timestamp { get; private set; }        

        public ILayerCanvasComponent FindComponent(ILayer layer)
        {
            Components.TryGetValue(layer, out var result);
            return result;
        }

        public ILayerCanvasComponent CreateComponent(ILayer layer, LayerRenderingOptions options)
        {
            var layerType = layer.GetType();
            if (!Reflection.LayerCanvasComponents.TryGetValue(layerType, out var componentType))
                throw new InvalidOperationException($"No canvas component found for {layerType}");
            var component = Activator.CreateInstance(componentType) as ILayerCanvasComponent;
            component.Initialize(layer, options, Timestamp);
            return Components[layer] = component;
        }

        public IEnumerable<ILayerCanvasComponent> GetFromSequence(Sequence seq, LayerRenderingOptions renderOpts)
        {
            //var renderOpts = seq.GetRenderingOptions();
            foreach (var layer in seq.Layers.Reverse<ILayer>()) 
                yield return FindComponent(layer) ?? CreateComponent(layer, renderOpts);
        }

        public IEnumerable<ICanvasComponent> GetFromSequenceManager(SequenceManager manager)
        {
            var seqs = new List<Sequence>();

            for(int i=manager.TracksCount-1;i>=0;i--)
            {                
                var track = manager.GetTrack(i);
                var seq = track.GetSequenceAtTimestamp(Timestamp);
                if (seq != null) seqs.Add(seq);                
            }
            if (seqs.Count == 0)
            {
                yield return new SimpleRectangle(new Rectangle(0, 0, 256, 192))
                {
                    Brush = FlipnotePaperColor.White.ToBrush(),
                    IsFixed = true
                };
                yield return new SimpleRectangle(new Rectangle(0, 0, 256, 192))
                {
                    Pen = Colors.FlipnoteThemeMainColor.GetPen(2, System.Drawing.Drawing2D.DashStyle.Dash),
                    IsFixed = true
                };
                yield break;
            }

            var renderOpts = seqs.Last().GetRenderingOptions();

            yield return new SimpleRectangle(new Rectangle(0, 0, 256, 192))
            {
                Brush = renderOpts.PaperColor.GetValueAt(Timestamp).ToBrush(),
                IsFixed = true
            };

            foreach (var seq in seqs)
                foreach (var comp in GetFromSequence(seq, renderOpts))
                    yield return comp;

            yield return new SimpleRectangle(new Rectangle(0, 0, 256, 192))
            {
                Pen = Colors.FlipnoteThemeMainColor.GetPen(2, System.Drawing.Drawing2D.DashStyle.Dash),
                IsFixed = true
            };            
        }

        public void UpdateTimestamp(int timestamp)
        {
            Timestamp = timestamp;
            foreach (var comp in Components.Values)
            {
                comp.Timestamp = Timestamp;
                comp.Refresh();
            }
        }      

    }
}