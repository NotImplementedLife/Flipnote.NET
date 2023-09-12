using System.Collections.Generic;

namespace FlipnoteDotNet.Data.StateChangeGenerators
{
    internal abstract class GradualStateChangeGenerator : IStateChangeGenerator
    {
        public int StartTimestamp { get; }
        public int EndTimestamp { get; }
        public int TimeStep { get; }

        protected GradualStateChangeGenerator(int startTimestamp, int endTimestamp, int timeStep)
        {
            StartTimestamp = startTimestamp;
            EndTimestamp = endTimestamp;
            TimeStep = timeStep;
        }
        protected GradualStateChangeGenerator(int startTimestamp, int endTimestamp) : this(startTimestamp, endTimestamp, 1) { }        

        public abstract IStateChanger GetStateChangerAt(int timestamp, int index);

        public IEnumerable<TemporalStateChanger> Generate()
        {
            for (int t = StartTimestamp, i = 0; t < EndTimestamp; t += TimeStep, i++)
                yield return new TemporalStateChanger(t, GetStateChangerAt(t, i));
        }
    }
}
