using FlipnoteDotNet.App.Canvas.Components;
using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.Canvas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FlipnoteDotNet.App.Storage
{
    public class FlipComponentDTO
    {
        [XmlIgnore]
        public CanvasTransform CanvasTransform { get; set; }
        public int AssetId { get; set; }

        public struct TransformDTO
        {
            [XmlAttribute]
            public int X { get; set; }
            [XmlAttribute]
            public int Y { get; set; }
            [XmlAttribute]
            public float ScaleX { get; set; }
            [XmlAttribute]
            public float ScaleY { get; set; }
            [XmlAttribute]
            public float AnchorX { get; set; }
            [XmlAttribute]
            public float AnchorY { get; set; }
            [XmlAttribute]
            public float Rotation { get; set; }
            public TransformDTO(CanvasTransform c)
            {
                X = c.X;
                Y = c.Y;
                ScaleX = c.ScaleX;
                ScaleY = c.ScaleY;
                AnchorX = c.AnchorX;
                AnchorY = c.AnchorY;
                Rotation = c.Rotation.Value;
            }

            public CanvasTransform ToCanvasTransform()
            {
                return new CanvasTransform(X, Y, Rotation, ScaleX, ScaleY, AnchorX, AnchorY);
            }
        }

        public TransformDTO Transform
        {
            get => new TransformDTO(CanvasTransform);
            set => CanvasTransform = value.ToCanvasTransform();
        }

        public FlipnoteCanvasComponent ToCanvasComponent(Func<int, Asset> idToAsset)
        {
            var asset = idToAsset(AssetId);
            var component = asset.CreateCanvasComponent();
            Debug.WriteLine($"Aid = {AssetId}");
            Debug.WriteLine($"Transform = {CanvasTransform}");
            component.Transform = CanvasTransform;
            return component;
        }
    }
}
