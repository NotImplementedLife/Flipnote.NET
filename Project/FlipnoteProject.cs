using FlipnoteDotNet.Data;

namespace FlipnoteDotNet.Project
{
    internal class FlipnoteProject
    {
        public string Path { get; private set; }
        public SequenceManager SequenceManager { get; private set; }

        public static FlipnoteProject CreateNew() => new FlipnoteProject { SequenceManager = new SequenceManager(5) };




    }
}
