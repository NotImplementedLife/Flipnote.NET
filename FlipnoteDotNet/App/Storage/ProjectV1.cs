using FlipnoteDotNet.App.Data;

namespace FlipnoteDotNet.App.Storage
{
    public class ProjectV1 : Project
    {
        public AssetDTO[] Assets { get; private set; } = new AssetDTO[0];
        public FrameDTO[] Frames { get; private set; } = new FrameDTO[0];
        public ProjectV1(IEnumerable<AssetDTO> assets) : base(formatVersion: 1)
        {
            Assets = assets.ToArray();

        }



    }
}
