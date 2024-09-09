using FlipnoteDotNet.App.Data;
using System.Xml.Serialization;

namespace FlipnoteDotNet.App.Storage
{
    [XmlInclude(typeof(ProjectV1))]
    public abstract class Project
    {
        public ushort FormatVersion { get; set; }

        protected Project(ushort formatVersion)
        {
            FormatVersion = formatVersion;
        }        
    }
}
