using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.App.Data.Assets;
using System.Xml.Serialization;

namespace FlipnoteDotNet.App.Storage.Assets
{
    public class StaticBitmapDTO : AssetDTO
    {

        public string ImageBytesRef { get; set; }        

        [XmlIgnore]
        public byte[] ImageBytes
        {
            get => BytesContainer.Get(ImageBytesRef);
            set => ImageBytesRef = BytesContainer.Put(value);
        }

        public StaticBitmapDTO() { }

        public StaticBitmapDTO(int id, string name, byte[] thumbnailBytes, byte[] imageBytes) : base(id, name, thumbnailBytes)
        {
            ImageBytes = imageBytes;
        }

        public override Asset ToAsset() => new StaticBitmap(ImageBytes, ThumbnailBytes, Name);
    }
}
