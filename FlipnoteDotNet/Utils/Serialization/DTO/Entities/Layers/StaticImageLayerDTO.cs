using FlipnoteDotNet.Data.Layers;
using PPMLib.Rendering;
using PPMLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Utils.Serialization.DTO.Entities.Layers
{
    public class StaticImageLayerDTO
    {
        public byte[] VisualSource { get; set; } 
        public string DisplayName { get; set; }
        public TimeDependentValueDTO<float> ScaleX { get; set; }
        public TimeDependentValueDTO<float> ScaleY { get; set; }
        public RescaleMethod RescaleMethod { get; set; }
        public bool Dithering { get; set; }


    }
}
