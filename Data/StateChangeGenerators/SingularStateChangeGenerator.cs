using System.Collections.Generic;

namespace FlipnoteDotNet.Data.StateChangeGenerators
{
    internal class SingularStateChangeGenerator : IStateChangeGenerator
    {
        private IStateChanger StateChanger { get; }
        private int Timestamp { get; }

        public SingularStateChangeGenerator(IStateChanger stateChanger, int timestamp)
        {
            StateChanger = stateChanger;
            Timestamp = timestamp;
        }

        public IEnumerable<TemporalStateChanger> Generate()
        {
            yield return new TemporalStateChanger(Timestamp, StateChanger);
        }
    }
}
