using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.Utils;
using FlipnoteDotNet.Core.GUI;
using System.ComponentModel;

namespace FlipnoteDotNet.App.Controls
{
    public class FramesViewer : Control
    {
        private FramesManager fFramesManager;
        private int FramesMargin = 10;

        private int fScroll = 0;        

        private readonly VirtualScroller VirtualScroller = new VirtualScroller(horizontal: true);

        public FramesViewer()
        {
            VirtualScroller.Attach(this);
            VirtualScroller.HScrollChanged += VirtualScroller_HScrollChanged;
        }

        private void VirtualScroller_HScrollChanged(object sender, int e)
        {
            fScroll = e;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (fFramesManager == null) { return; }
            var frameWidth = fFramesManager.CanvasModel.Width;
            var frameHeight = fFramesManager.CanvasModel.Height;
            float newHeight = Height - 2 * FramesMargin - 10;
            float newWidth = frameWidth * newHeight / frameHeight;

            int frameDx = (int)(newWidth + FramesMargin);
            int currentFrameIndex = fScroll / frameDx;
            int x = -fScroll % frameDx;
            
            int framesCount = FramesManager.Frames.Count;
            for (int i = currentFrameIndex; i < framesCount && x < Width; i++) 
            {
                var frameRect = new Rectangle(x, FramesMargin, (int)newWidth, (int)newHeight);
                if (Thumbnails.TryGetValue(FramesManager.Frames[i], out var thumbnail))
                {                    
                    if (thumbnail != null)
                        e.Graphics.DrawImage(thumbnail, frameRect);
                }
                if (FramesManager.Frames[i] == FramesManager.GetCurrentFrame())
                {
                    using (var pen = new Pen(Color.DarkOrange, 2))
                        e.Graphics.DrawRectangle(pen, frameRect);
                }
                else
                {
                    e.Graphics.DrawRectangle(Pens.Black, frameRect);
                }
                x += frameDx;
            }
            base.OnPaint(e);
        }

        public FramesManager FramesManager
        {
            get => fFramesManager;
            set
            {
                if (fFramesManager != null)
                    throw new InvalidOperationException("FramesManager already set");

                fFramesManager = value;
                if(fFramesManager.Frames is BindingList<Frame> bindingList)
                {
                    bindingList.ListChanged += FramesList_ListChanged;
                }
                fFramesManager.FrameThumbnailChanged += FFramesManager_FrameThumbnailChanged;

            }
        }

        private readonly Dictionary<Frame, Bitmap> Thumbnails = new Dictionary<Frame, Bitmap>();

        private void FFramesManager_FrameThumbnailChanged(object sender, Frame frame, Bitmap thumbnail)
        {
            if (!IsHandleCreated)
            {
                MainThreadInvoker.Invoke(UpdateThumbnailAction, this, frame, thumbnail);
            }
            else
            {
                Invoke(UpdateThumbnailAction, this, frame, thumbnail);
            }            
        }

        private readonly Action<FramesViewer, Frame, Bitmap> UpdateThumbnailAction = (framesViewer, frame, thumbnail) =>
        {
            if (framesViewer.Thumbnails.TryGetValue(frame, out var oldThumbnail))
            {
                framesViewer.Thumbnails[frame] = null;
                oldThumbnail?.Dispose();
            }
            framesViewer.Thumbnails[frame] = thumbnail;
            framesViewer.Invalidate();
        };

        private void FramesList_ListChanged(object sender, ListChangedEventArgs e)
        {
            var frameWidth = fFramesManager.CanvasModel.Width;
            var frameHeight = fFramesManager.CanvasModel.Height;
            float newHeight = Height - 2 * FramesMargin - 10;
            float newWidth = frameWidth * newHeight / frameHeight;
            int frameDx = (int)(newWidth + FramesMargin);
            VirtualScroller.SetScrollableWidth(FramesManager.Frames.Count * frameDx);
            VirtualScroller.SetScrollableHeight(2 * Height);
            Invalidate();            
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            var frameWidth = fFramesManager.CanvasModel.Width;
            var frameHeight = fFramesManager.CanvasModel.Height;
            float newHeight = Height - 2 * FramesMargin - 10;
            float newWidth = frameWidth * newHeight / frameHeight;
            int frameDx = (int)(newWidth + FramesMargin);
            int frameIndex = (fScroll + e.X) / frameDx;


            if (FramesMargin <= e.Y && e.Y < FramesMargin + newHeight) 
            {
                if (0 <= frameIndex && frameIndex < FramesManager.Frames.Count)
                {
                    FramesManager.SetCurrentFrame(frameIndex);
                    Invalidate();
                }
            }
        }
    }
}
