using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.App.Storage.Assets;
using System.Xml.Serialization;

namespace FlipnoteDotNet.App.Storage
{
    [XmlInclude(typeof(StaticBitmapDTO))]
    public abstract class AssetDTO
    {
        [XmlIgnore]
        public object Original { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }

        public string ThumbnailBytesRef { get; set; }

        [XmlIgnore]
        public byte[] ThumbnailBytes
        {
            get => BytesContainer.Get(ThumbnailBytesRef);
            set => ThumbnailBytesRef = BytesContainer.Put(value);
        }

        protected AssetDTO() { }

        protected AssetDTO(int id, string name, byte[] thumbnailBytes)
        {
            Id = id;
            Name = name;
            ThumbnailBytes = thumbnailBytes;
        }

        public abstract Asset ToAsset();
    }
}
