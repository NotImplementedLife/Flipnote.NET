using System;
using System.Collections.Generic;
using System.Text;

namespace PPMLib.Data
{
    public class FlipnoteFilename
    {
        public string Text { get; }

        public FlipnoteFilename(string text)
        {
            Text = text.Substring(0, 18).PadRight(18, ' ');
        }
    }
}
