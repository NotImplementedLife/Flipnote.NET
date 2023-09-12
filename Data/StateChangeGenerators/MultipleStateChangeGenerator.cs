using System.Collections.Generic;
using System.Linq;

namespace FlipnoteDotNet.Data.StateChangeGenerators
{
    internal class MultipleStateChangeGenerator : IStateChangeGenerator
    {
        private List<TemporalStateChanger> StateChangers = new List<TemporalStateChanger>();

        public MultipleStateChangeGenerator(List<TemporalStateChanger> stateChangers)
        {
            StateChangers = stateChangers.OrderBy(_ => _.Timestamp).ToList();
        }
        public IEnumerable<TemporalStateChanger> Generate() => StateChangers.AsEnumerable();
    }
}
