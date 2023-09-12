namespace FlipnoteDotNet.Data
{
    public interface ILayerStateChanger : IStateChanger
    {
        ILayer ChangeState(ILayer layer);
    }
}
