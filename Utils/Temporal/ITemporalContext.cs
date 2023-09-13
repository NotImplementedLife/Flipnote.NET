using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Utils.Temporal
{
    public interface ITemporalContext
    {
        int CurrentTimestamp { get; set; }
    }
}
