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
using System.Reflection;

namespace FlipnoteDesktop.Data
{
    public class Flipnote
    {        
        public Flipnote()
        {

        }

        public static _Metadata ReadMetadata(string filename)
        {
            _Metadata metadata = new _Metadata();
            using (BinaryReader r = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                r.BaseStream.Position = 0x10;
                metadata.Lock = r.ReadUInt16();                
                metadata.ThumbnailFrameIndex = r.ReadUInt16();                
                metadata.RootAuthorName = r.ReadWChars(11);                
                metadata.ParentAuthorName = r.ReadWChars(11);                
                metadata.CurrentAuthorName = r.ReadWChars(11);                
                metadata.ParentAuthorId = r.ReadBytes(8);                
                metadata.CurrentAuthorId = r.ReadBytes(8);                
                metadata.ParentFilename = r.ReadBytes(18);                
                metadata.CurrentFilename = r.ReadBytes(18);                
                metadata.RootAuthorId = r.ReadBytes(8);                
                metadata.RootFileFragment = r.ReadBytes(8);
                metadata.Timestamp = r.ReadUInt32();                                
            }
            return metadata;
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
                    throw new FileFormatException("Format version is not 0x24"); // just in case..
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

                //Debug.WriteLine(r.BaseStream.Position.ToString("X4"));
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

                //Debug.WriteLine(r.BaseStream.Position.ToString("X8"));

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

        public static List<Color> ThumbnailPalette = new List<Color>
        {
            (Color)ColorConverter.ConvertFromString("#FFFFFF"),
            (Color)ColorConverter.ConvertFromString("#525252"),
            (Color)ColorConverter.ConvertFromString("#FFFFFF"),
            (Color)ColorConverter.ConvertFromString("#9C9C9C"),
            (Color)ColorConverter.ConvertFromString("#FF4844"),
            (Color)ColorConverter.ConvertFromString("#C8514F"),
            (Color)ColorConverter.ConvertFromString("#FFADAC"),
            (Color)ColorConverter.ConvertFromString("#00FF00"),
            (Color)ColorConverter.ConvertFromString("#4840FF"),
            (Color)ColorConverter.ConvertFromString("#514FB8"),
            (Color)ColorConverter.ConvertFromString("#ADABFF"),
            (Color)ColorConverter.ConvertFromString("#00FF00"),
            (Color)ColorConverter.ConvertFromString("#B657B7"),
            (Color)ColorConverter.ConvertFromString("#00FF00"),
            (Color)ColorConverter.ConvertFromString("#00FF00"),
            (Color)ColorConverter.ConvertFromString("#00FF00")
        };

        public System.Drawing.Bitmap Thumbnail
        {
            get
            {
                var palette = new System.Drawing.Color[]
                {
                        System.Drawing.ColorTranslator.FromHtml("#FFFFFF"),
                        System.Drawing.ColorTranslator.FromHtml("#525252"),
                        System.Drawing.ColorTranslator.FromHtml("#FFFFFF"),
                        System.Drawing.ColorTranslator.FromHtml("#9C9C9C"),
                        System.Drawing.ColorTranslator.FromHtml("#FF4844"),
                        System.Drawing.ColorTranslator.FromHtml("#C8514F"),
                        System.Drawing.ColorTranslator.FromHtml("#FFADAC"),                        
                        System.Drawing.ColorTranslator.FromHtml("#00FF00"),
                        System.Drawing.ColorTranslator.FromHtml("#4840FF"),
                        System.Drawing.ColorTranslator.FromHtml("#514FB8"),
                        System.Drawing.ColorTranslator.FromHtml("#ADABFF"),
                        System.Drawing.ColorTranslator.FromHtml("#00FF00"),
                        System.Drawing.ColorTranslator.FromHtml("#B657B7"),
                        System.Drawing.ColorTranslator.FromHtml("#00FF00"),
                        System.Drawing.ColorTranslator.FromHtml("#00FF00"),
                        System.Drawing.ColorTranslator.FromHtml("#00FF00")                        
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
                                bmp.SetPixel(x, y, palette[RawThumbnail[offset] & 0x0F]);
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

        public string Filename;

        public static Flipnote New(string authorName, byte[] authorId, List<DecodedFrame> frames)
        {            
            var f = new Flipnote();
            f.FrameCount = (ushort)(frames.Count - 1);
            f.FormatVersion = 0x24;

            f.Metadata.RootAuthorId = new byte[8];
            f.Metadata.ParentAuthorId = new byte[8];
            f.Metadata.CurrentAuthorId = new byte[8];
            Array.Copy(authorId, f.Metadata.RootAuthorId, 8);
            Array.Copy(authorId, f.Metadata.ParentAuthorId, 8);
            Array.Copy(authorId, f.Metadata.CurrentAuthorId, 8);
            f.Metadata.RootAuthorName = authorName;
            f.Metadata.ParentAuthorName = authorName;
            f.Metadata.CurrentAuthorName = authorName;

            string mac6 = string.Join("", authorId.Take(3).Reverse().Select(t => t.ToString("X2")));
            var asm = Assembly.GetEntryAssembly().GetName().Version;
            var dt = DateTime.UtcNow;
            var H23 = asm.Major.ToString("X2");
            var H45 = asm.Minor.ToString("X2");
            var H67 = (dt.Year - 2009).ToString("X2");
            var H89 = dt.Month * 31 + dt.Day;
            var HABC = ((((dt.Hour * 3600 + dt.Minute * 60 + dt.Second) % 4096) >> 1) + (H89 > 255 ? 1 : 0)).ToString("X3");
            // just a placeholder till I find out how Flipnote Studio does actually generate file names             
            string _13str = $"80{H23}{H45}{H67}{H89.ToString("X2")}{HABC}";
            string nEdited = 0.ToString().PadLeft(3, '0');
            var filename = $"{mac6}_{_13str}_{nEdited}.ppm";
            f.Filename = filename;

            var rawfn = new byte[18];
            for (int i = 0; i < 3; i++) 
            {
                rawfn[i] = byte.Parse("" + mac6[2 * i] + mac6[2 * i + 1], System.Globalization.NumberStyles.HexNumber);
            }
            for (int i = 3; i < 16; i++)
            {
                rawfn[i] = (byte)_13str[i - 3];
            }
            rawfn[16] = rawfn[17] = 0;

            f.Metadata.ParentFilename = new byte[18];
            f.Metadata.CurrentFilename = new byte[18];

            Array.Copy(rawfn, f.Metadata.ParentFilename, 18);
            Array.Copy(rawfn, f.Metadata.CurrentFilename, 18);

            f.Metadata.RootFileFragment = new byte[8];
            for (int i = 0; i < 3; i++)
            {
                f.Metadata.RootFileFragment[i] = 
                    byte.Parse("" + mac6[2 * i] + mac6[2 * i + 1], System.Globalization.NumberStyles.HexNumber);
            }
            for (int i = 3; i < 8; i++) 
            {
                f.Metadata.RootFileFragment[i] =
                    (byte)((byte.Parse("" + _13str[2 * (i - 3)], System.Globalization.NumberStyles.HexNumber) << 4)
                          + byte.Parse("" + _13str[2 * (i - 3) + 1], System.Globalization.NumberStyles.HexNumber));
            }
            f.Metadata.Timestamp = (uint)((dt - new DateTime(2000, 1, 1, 0, 0, 0)).TotalSeconds);
            f.RawThumbnail = new DecodedFrame().CreateThumbnailW64();

            // write the animation data
            // THIS PART MUST BE CHANGED

            f.AnimationHeader.FrameOffsetTableSize = (ushort)(4 * frames.Count);
            f.AnimationHeader.Flags = 0x43;
            f.Frames = new _FrameData[frames.Count];
            for (int i = 0; i < frames.Count; i++)
            {
                f.Frames[i] = frames[i].ToFrameData();
            }


            return f;
        }

        public void Save(string fn)
        {
            using (var w = new BinaryWriter(new FileStream(fn, FileMode.Create)))
            {
                w.Write(FileMagic);
                w.Write(AnimationDataSize);                               
                w.Write(SoundDataSize);
                w.Write(FrameCount);
                w.Write((ushort)0x0024);
                w.Write(Metadata.Lock);
                w.Write(Metadata.ThumbnailFrameIndex);
                w.Write(Encoding.Unicode.GetBytes(Metadata.RootAuthorName.PadRight(11,'\0')));                
                w.Write(Encoding.Unicode.GetBytes(Metadata.ParentAuthorName.PadRight(11, '\0')));
                w.Write(Encoding.Unicode.GetBytes(Metadata.CurrentAuthorName.PadRight(11, '\0')));
                w.Write(Metadata.ParentAuthorId);
                w.Write(Metadata.CurrentAuthorId);
                w.Write(Metadata.ParentFilename);
                w.Write(Metadata.CurrentFilename);
                w.Write(Metadata.RootAuthorId);
                w.Write(Metadata.RootFileFragment);
                w.Write(Metadata.Timestamp);
                w.Write((ushort)0); //0x009E
                w.Write(RawThumbnail);                

                w.Write(AnimationHeader.FrameOffsetTableSize);
                w.Write((ushort)0); // 0x06A2
                w.Write(AnimationHeader.Flags);        
        /*                               
        /// #06A0
        AnimationHeader.FrameOffsetTableSize = r.ReadUInt16();
        /// #06A2
        AnimationHeader._06A2 = r.ReadUInt16();
        /// #06A6
        AnimationHeader.Flags = r.ReadUInt32();*/

    }
        }



        static readonly string checksumDict = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        static char FilenameChecksumDigit(string filename)
        {
            int sumc = Convert.ToInt32("" + filename[0] + filename[1], 16);
            for(int i=1;i<16;i++)
            {
                sumc = (sumc + (int)filename[i]) % 256;
            }
            return checksumDict[sumc % 36];
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

            public DateTime Date
            {
                get => new DateTime(2000, 1, 1).AddSeconds(Timestamp);                
            }
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
                int pos = (index & 0x3) * 2;
                return (LineEncoding)((_byte >> pos) & 0x3);
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

            /*public System.Drawing.Bitmap xFrame(int i)
            {
                var bmp = new System.Drawing.Bitmap(256, 192);
                for (int x = 0; x < 256; x++)
                    for (int y = 0; y < 192; y++)
                        bmp.SetPixel(x, y, Layer1[y, x] ? System.Drawing.Color.Black : System.Drawing.Color.White);
                return bmp;
            } */        

            public byte[] ToByte()
            {
                var res = new List<byte>();                
                res.Add(FirstByteHeader);
                // pack all lines with type 3 compression
                for (int l = 0; l < 192; l++)
                {
                    
                }
                return res.ToArray();
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
