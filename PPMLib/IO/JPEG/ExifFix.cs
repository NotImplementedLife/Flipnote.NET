using PPMLib.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PPMLib.IO.JPEG
{
    public static class ExifFix
    {
        // thumbnail: 160x120px
        public static JpgScan.IJpgComponent[] CreateMetadata(IEnumerable<JpgScan.IJpgComponent> components, byte[] thumbnailJpg)
        {
            var result = components.ToList();

            var i = result.Select((c, ix) => new { c, ix })
                .Where(g => g.c is JpgScan.JpgMarker m && (m.Type == 0xFFE0 || m.Type == 0xFFE1))
                .FirstOrDefault()?.ix ?? -1;

            var exif = Resources.exif_header.ToArray().Concat(thumbnailJpg).ToArray();
            var thumbnailSize = thumbnailJpg.Length & 0xFFFF;

            exif[0x22C] = (byte)(thumbnailSize >> 8);
            exif[0x22D] = (byte)(thumbnailSize & 0xFF);

            var marker = new JpgScan.JpgMarker(-1, 0xFFE1, (ushort)(exif.Length + 2));
            var data = new JpgScan.JpgData(-1, exif);

            if(i<0)
            {
                result.Insert(1, marker);
                result.Insert(2, data);

            }
            else
            {
                result[i] = marker;
                result[i + 1] = data;
            }

            return result.ToArray();
        }

    }
}
