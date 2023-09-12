using System.Collections.Generic;
using System.Linq;

namespace FlipnoteDotNet.Data.StateChangeGenerators
{
    public class CummulativeStateChangeGenerator : IStateChangeGenerator
    {
        public List<IStateChangeGenerator> Generators { get; } = new List<IStateChangeGenerator>();

        public IEnumerable<TemporalStateChanger> Generate() => Generators
            .Select(_ => _.Generate())
            .SelectMany(_ => _)
            .OrderBy(_ => _.Timestamp);
    }
}
