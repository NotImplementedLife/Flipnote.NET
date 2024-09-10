using FlipnoteDotNet.App.Canvas.Components;
using FlipnoteDotNet.Canvas;
using System.ComponentModel;
using System.Diagnostics;

namespace FlipnoteDotNet.App.Data
{
    public class FramesManager
    {
        private CanvasModel fCanvasModel = null;
        private CanvasModel fThumbnailCanvasModel;        
        public readonly PaletteConfig PaletteConfig;

        private readonly List<Frame> fFrames = new List<Frame>();
        private readonly BindingList<Frame> FramesBindingList;

        private Frame CurrentFrame = null;
        private int CurrentFrameIndex = -1;

        public FramesManager(PaletteConfig paletteConfig)
        {            
            PaletteConfig = paletteConfig;
            FramesBindingList = new BindingList<Frame>(fFrames);            
        }

        public CanvasModel CanvasModel
        {
            get => fCanvasModel;
            set
            {
                if(value==null && fCanvasModel!=null)
                {
                    fCanvasModel = null;
                    fThumbnailCanvasModel.Dispose();
                    fThumbnailCanvasModel=null;                    
                    return;
                }

                if (fCanvasModel != null)
                    throw new InvalidOperationException("CanvasModel has already been set");
                fCanvasModel = value;
                fThumbnailCanvasModel = new CanvasModel(fCanvasModel.Width, fCanvasModel.Height);
                fThumbnailCanvasModel.CanDebug = false;
            }
        }

        public IList<Frame> Frames => FramesBindingList;

        public void AddNewFrame()
        {
            var frame = CreateNewFrame();            
            Frames.Add(frame);            
            RequestThumbnailRedraw(frame);
        }        
        public void AddFrame(Frame frame)
        {            
            Frames.Add(frame);
            RequestThumbnailRedraw(frame);
        }

        public void RemoveFrame(int index)
        {
            if (CurrentFrame == Frames[index])
            {
                Frames.RemoveAt(index);
                SetCurrentFrame(index > 0 ? index - 1 : 0);
            }
            else
            {
                Frames.RemoveAt(index);
                if (index < CurrentFrameIndex)
                {
                    SetCurrentFrame(CurrentFrameIndex - 1);
                }
            }
        }

        public void InsertFrame(int index, Frame frame)
        {
            Frames.Insert(index, frame);
            SetCurrentFrame(index);
            //if (index <= CurrentFrameIndex)
            //CurrentFrameIndex++;
        }

        public Frame DuplicateCurrentFrame()
        {
            var frame = new Frame(fCanvasModel, PaletteConfig);
            frame.FrameConfig.ColorIndices = CurrentFrame.FrameConfig.ColorIndices?.ToArray();
            frame.FrameConfig.PaperColorIndex = CurrentFrame.FrameConfig.PaperColorIndex;
            var components = CurrentFrame.GetComponents();
            for (int i = 0; i < components.Length; i++)
            {
                frame.AddComponent(components[i].Clone() as FlipnoteCanvasComponent);
            }
            return frame;
        }

        public FrameConfig GetFrameConfig(int index) => fFrames[index].FrameConfig;

        public Frame GetCurrentFrame() => CurrentFrame;
        public Frame GetFrame(int index) => fFrames[index];
        public int GetCurrentFrameIndex() => CurrentFrameIndex;

        public void SetCurrentFrame(Frame frame, int index = -2)
        {
            if (CurrentFrame == frame) return;
            if(index==-2)
            {
                index = Frames.IndexOf(frame);
            }
            CurrentFrame?.DetachCanvas();
            CurrentFrame = frame;
            CurrentFrameIndex = index;
            CurrentFrame.AttachCanvas();
            CurrentFrameChanged?.Invoke(this, EventArgs.Empty);
        }

        public void SetCurrentFrame(int index) => SetCurrentFrame(index < 0 ? null : fFrames[index], index);


        public void EnsureCurrentFrameSelected()
        {
            if (CurrentFrame == null)
            {
                if (Frames.Count == 0)
                    AddFrame(CreateNewFrame());
                SetCurrentFrame(0);
            }
        }

        public Frame CreateNewFrame()
        {
            return new Frame(fCanvasModel, PaletteConfig);
        }        

        public void RequestThumbnailRedraw(Frame frame)
        {
            Task.Run(async () =>
            {
                var thumbnail = await frame.RenderThumbnail(fThumbnailCanvasModel);                
                FrameThumbnailChanged?.Invoke(this, frame, thumbnail);
            });            
        }

        public async Task RedrawThumbnailAsync(Frame frame)
        {
            var thumbnail = await frame.RenderThumbnail(fThumbnailCanvasModel);
            FrameThumbnailChanged?.Invoke(this, frame, thumbnail);
        }

        public delegate void OnFrameThumbnailChanged(object sender, Frame frame, Bitmap thumbnail);
        public event OnFrameThumbnailChanged FrameThumbnailChanged;

        public event EventHandler CurrentFrameChanged;

                
    }
}
