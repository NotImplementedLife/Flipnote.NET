using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Data
{
    public struct TemporalStateChanger : IStateChanger
    {
        public int Timestamp { get; }

        public IStateChanger StateChanger { get; }

        public TemporalStateChanger(int timestamp, IStateChanger stateChanger)
        {
            Timestamp = timestamp;
            StateChanger = stateChanger;
        }

        public ICloneable ChangeState(ICloneable item) => StateChanger.ChangeState(item);
    }
}
