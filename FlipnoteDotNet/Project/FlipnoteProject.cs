using FlipnoteDotNet.Data;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using FlipnoteDotNet.Utils.ProjectBinary;
using System.Windows.Forms;

namespace FlipnoteDotNet.Project
{
    [Serializable]
    public class FlipnoteProject
    {        
        public string Path { get; set; }
        public SequenceManager SequenceManager { get; private set; }

        public static FlipnoteProject CreateNew() => new FlipnoteProject { SequenceManager = new SequenceManager(5) };


        internal byte[] Serialize()
        {
            return ProjectBytes.Export(this);
        }      

    }
}
