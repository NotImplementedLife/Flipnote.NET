using FlipnoteDotNet.Constants;
using FlipnoteDotNet.Data;
using FlipnoteDotNet.GUI.Canvas;
using System;
using System.Collections.Generic;

namespace FlipnoteDotNet.Rendering
{
    internal class LayersManager
    {
        private Dictionary<ILayer, ILayerCanvasComponent> Components = new Dictionary<ILayer, ILayerCanvasComponent>();
        
        public int Timestamp { get; private set; }        

        public ILayerCanvasComponent FindOrCreateComponent(ILayer layer)
        {
            var layerType = layer.GetType();
            if(!Reflection.LayerCanvasComponents.TryGetValue(layerType, out var componentType))
            {
                throw new InvalidOperationException($"No canvas component found for {layerType}");
            }
            var component = Activator.CreateInstance(componentType, layer, Timestamp) as ILayerCanvasComponent;
            return component;
        }

        public void UpdateTimestamp(int timestamp)
        {
            Timestamp = timestamp;
            foreach (var comp in Components.Values)
            {
                comp.Timestamp = Timestamp;                
            }
        }

        public void InvalidateSequence(Sequence s)
        {

        }

    }
}
