using FlipnoteDotNet.App.Data;

namespace FlipnoteDotNet.App.Storage
{
    public class ProjectV1 : Project
    {
        public PaletteConfigDTO PaletteConfig { get; set; } = PaletteConfigs.Flipnote.ToDTO();
        public AssetDTO[] Assets { get; set; } = new AssetDTO[0];
        public FrameDTO[] Frames { get; set; } = new FrameDTO[0];
        public ProjectV1() : base(formatVersion: 1) { }        
    }
}
