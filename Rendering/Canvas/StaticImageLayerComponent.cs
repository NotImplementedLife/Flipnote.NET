﻿using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Data;
using FlipnoteDotNet.Data.Layers;
using FlipnoteDotNet.GUI.Canvas.Components;
using FlipnoteDotNet.GUI.Canvas.Drawing;
using System.Drawing;

namespace FlipnoteDotNet.Rendering.Canvas
{
    [CanvasComponent(typeof(StaticImageLayer))]
    internal class StaticImageLayerComponent : ILayerCanvasComponent
    {

        private BitmapComponent BitmapComponent;        

        public void Initialize(StaticImageLayer layer, LayerRenderingOptions options, int timestamp)
        {
            Layer = layer;
            LayerRenderingOptions = options;
            Timestamp = timestamp;
            BuildBitmapComponent();
        }

        void ILayerCanvasComponent.Initialize(ILayer layer, LayerRenderingOptions options, int timestamp)
            => Initialize(layer as StaticImageLayer, options, timestamp);

        private void BuildBitmapComponent()
        {
            BitmapComponent = new BitmapComponent(Layer.VisualSource
                .ToBitmap(LayerRenderingOptions.GetLayer1Color(Timestamp), LayerRenderingOptions.GetLayer2Color(Timestamp)));
        }

        public int Timestamp { get; set; }
        public StaticImageLayer Layer { get; private set; }
        ILayer ILayerCanvasComponent.Layer => Layer;

        public Rectangle Bounds 
        {
            get => new Rectangle(Location, Size);
            set
            {
                Location = Bounds.Location;
                Size = Bounds.Size;
            }
        }
        public Point Location 
        {
            get => new Point(Layer.X.GetValueAt(Timestamp), Layer.Y.GetValueAt(Timestamp));
            set
            {
                Layer.X.PutCurrentConstantTransformer(value.X, Timestamp, autoUpdate: true);
                Layer.Y.PutCurrentConstantTransformer(value.Y, Timestamp, autoUpdate: true);
            }                
        }
        public Size Size 
        {
            get => new Size((int)(Layer.ScaleX.GetValueAt(Timestamp) * Layer.VisualSource.Width),
                    (int)(Layer.ScaleY.GetValueAt(Timestamp) * Layer.VisualSource.Height));
            set
            {
                var w = Layer.VisualSource.Width == 0 ? 1.0f : 1.0f * value.Width / Layer.VisualSource.Width;
                var h = Layer.VisualSource.Height == 0 ? 1.0f : 1.0f * value.Height / Layer.VisualSource.Height;
                Layer.ScaleX.PutCurrentConstantTransformer(w, Timestamp, autoUpdate: true);
                Layer.ScaleY.PutCurrentConstantTransformer(h, Timestamp, autoUpdate: true);
            }
        }
        public bool IsFixed { get; set; }
        public bool IsResizeable { get; set; } = true;

        public void OnPaint(CanvasGraphics g)
        {
            BitmapComponent.Location = new Point(Layer.X, Layer.Y);
            BitmapComponent.Size = Size;
            BitmapComponent.OnPaint(g);
        }

        private LayerRenderingOptions LayerRenderingOptions;

        public void Refresh()
        {
            BuildBitmapComponent();
        }        
    }
}