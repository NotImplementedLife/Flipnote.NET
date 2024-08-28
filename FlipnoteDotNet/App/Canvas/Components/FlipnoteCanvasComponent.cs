using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.Canvas.Components;

namespace FlipnoteDotNet.App.Canvas.Components
{
    public abstract class FlipnoteCanvasComponent : CanvasComponent
    {
        protected FrameConfig FrameConfig = null;
        protected Frame Frame = null;
        protected Asset Asset = null;        

        protected FlipnoteCanvasComponent(string name, int width, int height, bool ignoreTransform, bool ignoreGraphicsTransform = false,
            Asset asset = null)
            : base(name, width, height, ignoreTransform, ignoreGraphicsTransform)
        {
            Asset = asset;
        }

        public void AttachToFrame(Frame frame)
        {
            if(frame==null)
            {
                if (Frame != null)
                {
                    OnDetachedFromFrame();             
                }
                Frame = null;
            }
            else
            {
                if (Frame != null)
                    throw new InvalidOperationException("Component already attached to a Frame");
                Frame = frame;
                OnAttachedToFrame(Frame);
            }
        }

        public void DetachFromFrame() => AttachToFrame(null);
        
        protected virtual void OnAttachedToFrame(Frame frame) 
        {
            FrameConfig = frame.FrameConfig;
        }
        protected virtual void OnDetachedFromFrame()
        { 
            FrameConfig = null;
        }

        public override CanvasComponent Clone()
        {
            if (Asset != null)
            {
                var clone = Asset.CreateCanvasComponent();
                clone.FrameConfig = FrameConfig;
                clone.Transform = Transform;
                return clone;
            }
            throw new NotImplementedException("Component was not created from Asset and no Clone() override was specified");
        }
    }
}
