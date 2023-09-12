namespace FlipnoteDotNet.Data.Layers
{
    public abstract class AbstractLayer : ILayer
    {
        public abstract ILayer Clone();
        ICloneable ICloneable.Clone() => Clone();
    }
}
