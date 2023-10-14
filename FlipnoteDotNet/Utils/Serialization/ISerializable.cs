namespace FlipnoteDotNet.Utils.Serialization
{
    public interface ISerializable
    {
        void Serialize(SerializeWriter w);
        void Deserialize(SerializeReader r, object target, bool isInitialized);
    }
}
