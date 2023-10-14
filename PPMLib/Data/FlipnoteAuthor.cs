using System;
using System.Collections.Generic;
using System.Text;

namespace PPMLib.Data
{
    public struct FlipnoteAuthor
    {
        public string Name { get; }
        public ulong Id { get; }

        public FlipnoteAuthor(string name, ulong id)
        {
            Name = name.Substring(0, 11).PadRight(11, ' ');
            Id = id;
        }
    }
}
