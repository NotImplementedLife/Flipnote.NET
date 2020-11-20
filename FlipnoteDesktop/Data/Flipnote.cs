using FlipnoteDesktop.Extensions;
using FlipnoteDesktop.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FlipnoteDesktop.Data
{
    public class Flipnote
    {        
        public Flipnote()
        {

        }

        public Flipnote(string filename)
        {
            using (BinaryReader r = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                /// #0000
                if (r.ReadChars(4).Equals(FileMagic)) 
                    throw new FileFormatException("Unexpected file format.");
                /// #0004
                AnimationDataSize = r.ReadUInt32();
                /// #0008
                SoundDataSize = r.ReadUInt32();
                /// #000C
                FrameCount = r.ReadUInt16();
                /// #000E
                FormatVersion = r.ReadUInt16();
                if (FormatVersion != 0x24)
                    throw new FileFormatException("Format version does not match. This may be a sign that the file is corrupted");
                /// #0010
                Metadata.Lock = r.ReadUInt16();
                /// #0012
                Metadata.ThumbnailFrameIndex = r.ReadUInt16();
                /// #0014
                Metadata.RootAuthorName = r.ReadWChars(11);
                /// #002A
                Metadata.ParentAuthorName = r.ReadWChars(11);
                /// #0040
                Metadata.CurrentAuthorName = r.ReadWChars(11);
                /// #0056
                Metadata.ParentAuthorId = r.ReadBytes(8);
                /// #005E
                Metadata.CurrentAuthorId = r.ReadBytes(8);
                /// #0066
                Metadata.ParentFilename = r.ReadBytes(18);
                /// #0078
                Metadata.CurrentFilename = r.ReadBytes(18);
                /// #008A
                Metadata.RootAuthorId = r.ReadBytes(8);
                /// #0092
                Metadata.RootFileFragment = r.ReadBytes(8);
                /// #009A
                Metadata.Timestamp = r.ReadUInt32();
                /// #009E
                Metadata._0x9E = r.ReadUInt16();
                /// #00A0
                RawThumbnail = r.ReadBytes(1536);
                /// #06A0
                AnimationHeader.FrameOffsetTableSize = r.ReadUInt16();
                /// #06A2
                AnimationHeader._06A2 = r.ReadUInt16();
                /// #06A6
                AnimationHeader.Flags = r.ReadUInt32();
                /// #06A8
                AnimationHeader.Offsets = new uint[AnimationHeader.FrameOffsetTableSize / 4];
                int len = AnimationHeader.FrameOffsetTableSize / 4;
                for (int i = 0; i < len; i++)                
                    AnimationHeader.Offsets[i] = r.ReadUInt32();
                
                long framesPos0 = r.BaseStream.Position;
                Frames = new _FrameData[len];                
                long offset = 0x06A8 + AnimationHeader.FrameOffsetTableSize;
                for (int i = 0; i < len; i++)
                {                    
                    r.BaseStream.Seek(offset + AnimationHeader.Offsets[i], SeekOrigin.Begin);
                    Frames[i] = r.ReadPPMFrameData(0x06A8 + AnimationHeader.FrameOffsetTableSize);                    
                    Frames[i].AnimationIndex = Array.IndexOf(AnimationHeader.Offsets, Frames[i].Position);
                    if (i > 0)
                    {
                        Frames[i].Overwrite(Frames[i - 1]);                        
                    }
                }                
                RenderFrame(0);

                offset = 0x6A0 + AnimationDataSize;
                r.BaseStream.Seek(offset, SeekOrigin.Begin);
                SoundEffectFlags = new byte[Frames.Length];
                for (int i = 0; i < Frames.Length; i++)
                {
                    SoundEffectFlags[i] = r.ReadByte();                    
                }
                offset += Frames.Length;
                /// make the next offset dividable by 4
                r.ReadBytes((int)((4 - offset % 4) % 4));

                Debug.WriteLine(r.BaseStream.Position.ToString("X4"));
                SoundHeader.BGMTrackSize = r.ReadUInt32();
                SoundHeader.SE1TrackSize = r.ReadUInt32();
                SoundHeader.SE2TrackSize = r.ReadUInt32();
                SoundHeader.SE3TrackSize = r.ReadUInt32();
                SoundHeader.CurrentFramespeed = r.ReadByte();
                SoundHeader.RecordedBGMFramespeed = r.ReadByte();
                r.ReadBytes(14);

                SoundData.RawBGM = r.ReadBytes((int)SoundHeader.BGMTrackSize);
                SoundData.RawSE1 = r.ReadBytes((int)SoundHeader.SE1TrackSize);
                SoundData.RawSE2 = r.ReadBytes((int)SoundHeader.SE2TrackSize);
                SoundData.RawSE3 = r.ReadBytes((int)SoundHeader.SE3TrackSize);

                Debug.WriteLine(r.BaseStream.Position.ToString("X8"));

                /// Next 0x80 bytes = RSA-1024 SHA-1 signature
                /// Next 0x10 bytes are filled with 0
            }
        }

        public readonly char[] FileMagic = new char[4] { 'P', 'A', 'R', 'A' };
        public uint AnimationDataSize;
        public uint SoundDataSize;
        public ushort FrameCount;
        public ushort FormatVersion;
        public _Metadata Metadata = new _Metadata();
        public byte[] RawThumbnail;
        public _AnimationHeader AnimationHeader = new _AnimationHeader();
        public _FrameData[] Frames;
        public byte[] SoundEffectFlags;
        public _SoundHeader SoundHeader = new _SoundHeader();
        public _SoundData SoundData = new _SoundData();

        public List<DecodedFrame> GetDecodedFrameList()
        {
            var result = new List<DecodedFrame>();
            for (int i = 0; i < Frames.Length; i++)
            {
                var df = new DecodedFrame();
                for (int x = 0; x < 256; x++)
                    for (int y = 0; y < 192; y++)
                    {
                        df.Layer1Data[x, y] = Frames[i].Layer1[y, x];
                        df.Layer2Data[x, y] = Frames[i].Layer2[y, x];
                    }              
                int f = (Frames[i].FirstByteHeader & 0b00000110) >> 1;
                if (f > 0)
                    df.Layer1Color = (LayerColor)(f - 1);
                f = (Frames[i].FirstByteHeader & 0b00011000) >> 3;
                if (f > 0)
                    df.Layer2Color = (LayerColor)(f - 1);
                df.IsPaperWhite = (Frames[i].FirstByteHeader & 1) == 1;
                df.SetImage(null, true);
                result.Add(df);
            }

            return result;
        }

        public System.Drawing.Bitmap Thumbnail
        {
            get
            {
                var palette = new System.Drawing.Color[]
                {
                        System.Drawing.Color.White,
                        System.Drawing.Color.DarkGray,
                        System.Drawing.Color.White,
                        System.Drawing.Color.LightGray,
                        System.Drawing.Color.Red,
                        System.Drawing.Color.DarkRed,
                        System.Drawing.Color.Pink,
                        System.Drawing.Color.Green,
                        System.Drawing.Color.Blue,
                        System.Drawing.Color.DarkBlue,
                        System.Drawing.Color.LightBlue,
                        System.Drawing.Color.Magenta,
                        System.Drawing.Color.Green,
                        System.Drawing.Color.Green,
                        System.Drawing.Color.Green,
                };
                var bmp = new System.Drawing.Bitmap(64, 48);
                int offset = 0;
                for (int ty = 0; ty < 48; ty += 8)
                    for (int tx = 0; tx < 64; tx += 8)
                        for (int l = 0; l < 8; l++)
                            for (int px = 0; px < 8; px += 2)
                            {
                                int x = tx + px;
                                int y = ty + l;
                                bmp.SetPixel(x, y, palette[RawThumbnail[offset]&0x0F]);
                                bmp.SetPixel(x+1, y, palette[(RawThumbnail[offset] >> 4) & 0x0F]);
                                offset++;
                            }
                return bmp;
            }
        }

        public WriteableBitmap RenderFrame(int index)
        {            
            var palette = new BitmapPalette(new List<Color>
            {
                Frames[index].PaperColor,
                Frames[index].Frame1Color,
                Frames[index].Frame2Color
            });
            var bmp = new WriteableBitmap(256, 192, 96, 96, PixelFormats.Indexed2, palette);

            int stride = 64;
            byte[] pixels = new byte[64 * 192];
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 192; y++)
                {
                    if (Frames[index].Layer2[y, x])
                    {
                        int b = 256 * y + x;
                        int p = 3 - b % 4;
                        b /= 4;
                        pixels[b] &= (byte)(~(0b11 << (2 * p)));
                        pixels[b] |= (byte)(0b10 << (2 * p));
                    }
                }
            }
            for (int x=0;x<256;x++)
            {
                for (int y = 0; y < 192; y++) 
                {
                    if (Frames[index].Layer1[y, x]) 
                    {
                        int b = 256 * y + x;
                        int p = 3 - b % 4;
                        b /= 4;
                        pixels[b] &= (byte)(~(0b11 << (2 * p)));
                        pixels[b] |= (byte)(0b01 << (2 * p));
                    }
                }
            }            
            bmp.WritePixels(new System.Windows.Int32Rect(0, 0, 256, 192), pixels, stride, 0);           
            return bmp;
        }

        public class _Metadata
        {
            public ushort Lock;
            public ushort ThumbnailFrameIndex;
            public string RootAuthorName;
            public string ParentAuthorName;
            public string CurrentAuthorName;
            public byte[] ParentAuthorId;
            public byte[] CurrentAuthorId;
            public byte[] ParentFilename;
            public byte[] CurrentFilename;
            public byte[] RootAuthorId;
            public byte[] RootFileFragment;
            public uint Timestamp;
            public ushort _0x9E; //unused

            public string fn(byte[] a)
            {
                return string.Join("", new byte[] { a[0], a[1], a[2] }.Select(b => b.ToString("X2"))) + "_" +
                    string.Join("", new char[] { (char)a[3],(char)a[4],(char)a[5], (char)a[6], (char)a[7], (char)a[8],
                    (char)a[9],(char)a[10],(char)a[11],(char)a[12],(char)a[13],(char)a[14],(char)a[15] }) + "_" +
                    ((ushort)a[17] * 16 + a[16]).ToString("000");
            }

            public string ParentFilenameStr() => fn(ParentFilename);
            public string CurrentFilenameStr() => fn(CurrentFilename);     
           
        }        

        public class _AnimationHeader
        {
            public ushort FrameOffsetTableSize;
            public ushort _06A2; // 0
            public uint Flags;  // ???
            public uint[] Offsets;
        }

        public class _FrameData
        {
            public long Position;            
            public long AnimationIndex;
            public byte FirstByteHeader;
            public byte[] Layer1LineEncoding;
            public byte[] Layer2LineEncoding;

            public bool[,] Layer1 = new bool[192, 256];
            public bool[,] Layer2 = new bool[192, 256];

            public int TranslateX = 0;
            public int TranslateY = 0;
            
            public Color PaperColor
            {
                get => (FirstByteHeader & 1) == 1 ? Colors.White : Colors.Black;
            }

            public Color Frame1Color
            {
                get
                {
                    int flag1 = (FirstByteHeader & 0b00000110) >> 1;
                    if (flag1 == 2) return Colors.Red;
                    if (flag1 == 3) return Colors.Blue;
                    if (flag1 == 1)
                        return (FirstByteHeader & 1) == 1 ? Colors.Black : Colors.White;
                    return Colors.Transparent;
                }
            }

            public Color Frame2Color
            {
                get
                {
                    int flag2 = (FirstByteHeader & 0b00011000) >> 3;
                    if (flag2 == 2) return Colors.Red;
                    if (flag2 == 3) return Colors.Blue;
                    if (flag2 == 1)
                        return (FirstByteHeader & 1) == 1 ? Colors.Black : Colors.White;
                    return Colors.Transparent;
                }
            }

            public LineEncoding GetLineEncoding1(int index)
            {
                int _byte = Layer1LineEncoding[index >> 2];
                int pos = (index & 0x3) * 2;
                return (LineEncoding)((_byte >> pos) & 0x3);               
            }

            public LineEncoding GetLineEncoding2(int index)
            {
                int _byte = Layer2LineEncoding[index >> 2];
                int pos = (index & 0x03) * 2;
                return (LineEncoding)((_byte >> pos) & 0x03);
            }

            public void Overwrite(_FrameData frame)
            {                
                if ((FirstByteHeader & 0b10000000) != 0)
                {                    
                    return;
                }                
                for(int y=0;y<192;y++)
                {
                    if (y - TranslateY < 0) continue;
                    if (y - TranslateY >= 192) break;
                    for(int x=0;x<256;x++)
                    {
                        if (x - TranslateX < 0) continue;
                        if (x - TranslateX >= 256) break;
                        Layer1[y, x] ^= frame.Layer1[y - TranslateY, x - TranslateX];
                        Layer2[y, x] ^= frame.Layer2[y - TranslateY, x - TranslateX];
                    }
                }
            }

            public System.Drawing.Bitmap xFrame(int i)
            {
                var bmp = new System.Drawing.Bitmap(256, 192);
                for (int x = 0; x < 256; x++)
                    for (int y = 0; y < 192; y++)
                        bmp.SetPixel(x, y, Layer1[y, x] ? System.Drawing.Color.Black : System.Drawing.Color.White);
                return bmp;
            }         
        }

        public class _SoundHeader
        {
            public uint BGMTrackSize;
            public uint SE1TrackSize;            
            public uint SE2TrackSize;            
            public uint SE3TrackSize;           
            public byte CurrentFramespeed;
            public byte RecordedBGMFramespeed;
        }         

        public class _SoundData
        {
            public byte[] RawBGM;
            public byte[] RawSE1;
            public byte[] RawSE2;
            public byte[] RawSE3;
        }

        public enum LineEncoding
        {
            SkipLine = 0,
            CodedLine = 1,
            InvertedCodedLine = 2,
            RawLineData = 3
        }

        public int FrameSpeed
        {
            get => PlaybackSpeed[SoundHeader.CurrentFramespeed];
        }

        public static _PlaybackSpeed PlaybackSpeed = new _PlaybackSpeed();
        public class _PlaybackSpeed
        {
            public int this[int i]
            {
                get
                {
                    switch (8-i)
                    {
                        case 1: return 2000;
                        case 2: return 1000;
                        case 3: return 500;
                        case 4: return 250;
                        case 5: return 166;
                        case 6: return 83;
                        case 7: return 50;
                        case 8: return 33;
                        default: return 33;
                    }
                }
            }
        }
    }
}
