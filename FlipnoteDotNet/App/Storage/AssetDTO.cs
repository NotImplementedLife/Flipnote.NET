namespace FlipnoteDotNet.App.Storage
{
    public abstract class AssetDTO
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public byte[] ThumbnailBytes { get; private set; }

        protected AssetDTO(int id, string name, byte[] thumbnailBytes)
        {
            Id = id;
            Name = name;
            ThumbnailBytes = thumbnailBytes;
        }
    }
}
