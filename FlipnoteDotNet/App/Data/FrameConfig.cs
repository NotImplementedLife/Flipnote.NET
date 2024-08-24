using FlipnoteDotNet.Drawing;

namespace FlipnoteDotNet.App.Data
{
    public class FrameConfig
    {
        public readonly Palette Palette;

        private int[] fColorIndices = null;

        private Palette fActualPalette;
        public Palette ActualPalette { get => fActualPalette; }
        public Color PaperColor { get; private set; }

        private int fPaperColorIndex = 0;

        public int PaperColorIndex
        {
            get => fPaperColorIndex;
            set
            {
                if (fPaperColorIndex == value) return;
                fPaperColorIndex = value;
                PaperColor = fActualPalette.Colors[fPaperColorIndex];
                PaperColorChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int[] ColorIndices
        {
            get => fColorIndices;
            set
            {
                if (fColorIndices == null || value == null) return;
                if (fColorIndices.Length != value.Length)
                    throw new ArgumentException("Invalid color indices array.");
                var oldPaperColor = fActualPalette.Colors[PaperColorIndex].ToArgb();
                fColorIndices = value;
                fActualPalette = fColorIndices == null ? Palette : Palette.IndexedSubpalette(fColorIndices);                
                var newPaperColor = fActualPalette.Colors[PaperColorIndex].ToArgb();
                if (oldPaperColor != newPaperColor)
                {
                    PaperColor = fActualPalette.Colors[fPaperColorIndex];
                    PaperColorChanged?.Invoke(this, EventArgs.Empty);
                }
                ColorIndicesChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public FrameConfig(PaletteConfig paletteConfig) 
        {
            Palette = paletteConfig.Palette;
            if (paletteConfig.ColorsPerFrame != 0)
            {
                fColorIndices = new int[paletteConfig.ColorsPerFrame];
                for (int i = 0; i < fColorIndices.Length; i++) fColorIndices[i] = i;
            }
            fActualPalette = fColorIndices == null ? Palette : Palette.IndexedSubpalette(fColorIndices);
            PaperColor = fActualPalette.Colors[fPaperColorIndex];
        }

        public event EventHandler ColorIndicesChanged;

        public event EventHandler PaperColorChanged;
        
    }
}
