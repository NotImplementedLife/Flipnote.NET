using FlipnoteDotNet.App.Canvas.Components;
using FlipnoteDotNet.App.Storage;
using FlipnoteDotNet.Canvas;
using FlipnoteDotNet.Canvas.Components;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;

namespace FlipnoteDotNet.App.Data
{
    public class Frame
    {
        private readonly CanvasModel CanvasModel;
        public readonly FrameConfig FrameConfig;
        internal readonly IList<CanvasComponent> Components = new BindingList<CanvasComponent>();

        public Frame(CanvasModel canvasModel, PaletteConfig paletteConfig)
        {
            CanvasModel = canvasModel;
            FrameConfig = new FrameConfig(paletteConfig);            
        }

        private bool IsCanvasAttached = false;

        internal CanvasComponent[] GetComponents() => Components.ToArray();

        public void AttachCanvas()
        {
            CanvasModel.BindComponentsList(Components);
            CanvasModel.SetBackgroundColor(FrameConfig.PaperColor);            
            FrameConfig.PaperColorChanged += FrameConfig_PaperColorChanged;            
            IsCanvasAttached = true;
        }

        public void DetachCanvas()
        {
            IsCanvasAttached = false;
            FrameConfig.PaperColorChanged -= FrameConfig_PaperColorChanged;
        }

        public void AddComponent(FlipnoteCanvasComponent component)
        {
            component.AttachToFrame(this);
            if (IsCanvasAttached)
                CanvasModel.AddComponent(component);
            else
                Components.Add(component);            
        }

        public void InsertComponent(int index, FlipnoteCanvasComponent component)
        {
            component.AttachToFrame(this);
            if (IsCanvasAttached)
                CanvasModel.InsertComponent(index, component);
            else
                Components.Insert(index, component);
        }

        public void SwapComponents(int i, int j)
        {
            if (IsCanvasAttached)
            {
                CanvasModel.SwapComponents(i, j);
            }
            else
            {
                (Components[i], Components[j]) = (Components[j], Components[i]);
            }
        }

        public void ClearComponents()
        {
            if(IsCanvasAttached)
            {
                var comps = Components.ToList();
                CanvasModel.Clear();
                for (int i = 0; i < comps.Count; i++)
                    (comps[i] as FlipnoteCanvasComponent).DetachFromFrame();
            }
            else
            {
                for (int i = 0; i < Components.Count; i++)
                    (Components[i] as FlipnoteCanvasComponent).DetachFromFrame();
                Components.Clear();
            }
        }

        public bool RemoveComponent(FlipnoteCanvasComponent component)
        {
            if (IsCanvasAttached)
            {
                bool removed = CanvasModel.RemoveComponent(component);
                if (removed)
                    component.DetachFromFrame();
                return removed;
            }
            else
            {
                bool removed = Components.Remove(component);
                if (removed) component.DetachFromFrame();
                return removed;               
            }
        }

        private void FrameConfig_PaperColorChanged(object sender, EventArgs e)
        {
            CanvasModel.SetBackgroundColor(FrameConfig.PaperColor);
        }

        public int PaperColorIndex
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => FrameConfig.PaperColorIndex;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => FrameConfig.PaperColorIndex = value;
        }

        public Task<Bitmap> RenderThumbnail(CanvasModel canvasModel) => Task.Run<Bitmap>(() =>
        {
            Bitmap result = new Bitmap(CanvasModel.Width / 4, CanvasModel.Height / 4);
            lock (canvasModel)
            {
                canvasModel.Clear();
                canvasModel.SetBackgroundColor(FrameConfig.PaperColor);
                var components = GetComponents();
                for (int i = 0; i < components.Length; i++)
                {
                    canvasModel.AddComponent(components[i].Clone());
                }
                canvasModel.Update(force: true);
                using(var g = Graphics.FromImage(result))
                {
                    g.InterpolationMode = InterpolationMode.Bilinear;
                    g.PixelOffsetMode = PixelOffsetMode.Half;
                    canvasModel.DrawOnGraphics(g, 0, 0, result.Width, result.Height);
                }
            }
            return result;
        });

        public FrameDTO ToDTO()
        {
            var components = from c in Components
                             where c is FlipnoteCanvasComponent
                             let fc = c as FlipnoteCanvasComponent
                             select new FlipComponentDTO { CanvasTransform = fc.Transform, AssetId = fc.SourceAsset.Id };
            return new FrameDTO
            {
                ColorIndices = FrameConfig.ColorIndices.ToArray(),
                PaperColorIndex = FrameConfig.PaperColorIndex,
                Components = components.ToArray()
            };
        }
    }
}
