using System.Collections.Generic;

namespace FlipnoteDotNet.Data
{
    public interface IStateChangeGenerator
    {
        IEnumerable<TemporalStateChanger> Generate();
    }
}
