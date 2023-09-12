namespace FlipnoteDotNet.Data
{
    public interface IStateChanger
    {
        ICloneable ChangeState(ICloneable item);
    }
}
