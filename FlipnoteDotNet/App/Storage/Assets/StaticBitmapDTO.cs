namespace FlipnoteDotNet.App.Storage.Assets
{
    public class StaticBitmapDTO : AssetDTO
    {
        public byte[] ImageBytes { get; private set; }

        public StaticBitmapDTO(int id, string name, byte[] thumbnailBytes, byte[] imageBytes) : base(id, name, thumbnailBytes)
        {
            ImageBytes = imageBytes;
        }
    }
}
