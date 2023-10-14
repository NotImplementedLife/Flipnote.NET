﻿using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.GUI.Forms.LayerCreators;
using FlipnoteDotNet.Utils;
using FlipnoteDotNet.Utils.Temporal;
using PPMLib.Rendering;
using PPMLib.Utils;
using System;
using System.Diagnostics;
using System.Drawing;

namespace FlipnoteDotNet.Data.Layers
{    
    [Layer(DisplayName = "Static image layer", CreatorForm = typeof(StaticImageLayerCreatorForm))]
    [Serializable]
    internal class StaticImageLayer : LocalizableLayer, IDisplayLayer
    {
        private FlipnoteVisualSource _VisualSource = null;

        [Editable]
        [DisplayName("Visual")]
        public FlipnoteVisualSource VisualSource
        {
            get => _VisualSource;
            set
            {
                _VisualSource = value;
                Debug.WriteLine("Visual source changed");
                _DisplayThumbnail.Redraw(40, 40);
            }
        }

        [Editable]
        [DisplayName("Name")]
        public string DisplayName { get; set; }


        [Editable]
        public TimeDependentValue<float> ScaleX { get; }

        [Editable]
        public TimeDependentValue<float> ScaleY { get; }

        [Editable]
        public RescaleMethod RescaleMethod { get; set; } = RescaleMethod.NearestNeighbor;

        [Editable]
        public bool Dithering { get; set; } = false;
        
        private AsyncBitmap _DisplayThumbnail;

        public Bitmap GetDisplayThumbnail()
        {
            return _DisplayThumbnail.DisplayBitmap.Clone() as Bitmap;
        }

        public StaticImageLayer(FlipnoteVisualSource visualSource)
        {
            _DisplayThumbnail = new AsyncBitmap(RenderBitmap);
            _DisplayThumbnail.Ready += _DisplayThumbnail_Ready;
            VisualSource = visualSource;
            ScaleX = new TimeDependentValue<float>(this, 1);
            ScaleY = new TimeDependentValue<float>(this, 1);
            this.Initialize();                                    
        }

        private void _DisplayThumbnail_Ready(object sender, EventArgs e)
        {
            Debug.WriteLine("DT ready");
            DisplayChanged?.Invoke(this, new EventArgs());
        }

        private void RenderBitmap(Graphics g, Rectangle bounds)
        {
            if (VisualSource == null) return;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            g.DrawImage(VisualSource.ToBitmap(Color.Black, Color.Gray), VisualSource.Size.ScaleToFit(new Rectangle(0, 0, 40, 40)));
        }

        public event EventHandler DisplayChanged;        
    }
}