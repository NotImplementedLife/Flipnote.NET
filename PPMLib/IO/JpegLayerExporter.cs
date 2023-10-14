using PPMLib.Data;
using PPMLib.Extensions;
using PPMLib.IO.JPEG;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static PPMLib.Utils.Memory;
using static System.Net.Mime.MediaTypeNames;

namespace PPMLib.IO
{
    public abstract class JpegLayerExporter
    {
        public byte[] Key { get; } = new byte[16];
        public byte[] IV { get; } = new byte[12];

        public JpegLayerExporter(byte[] commonKey, byte[] iv = null)
        {
            Array.Copy(commonKey, Key, Math.Min(commonKey.Length, Key.Length));
            iv = iv ?? GenerateNonce();            

            Array.Copy(iv, IV, Math.Min(iv.Length, IV.Length));
            Debug.WriteLine(Key.Select(_=>$"{_:X2}").JoinToString(" "));

        }

        protected abstract byte[] GetLayerJpg(FlipnoteFrameLayer layer);

        protected abstract byte[] GetLayerJpgThumbnail(FlipnoteFrameLayer layer);

        public byte[] Export(FlipnoteFrameLayer layer)
        {
            var jpg = GetLayerJpg(layer);
            var comps = JpgScan.DissectJpg(jpg);
            var thumbnail = GetLayerJpgThumbnail(layer);
            comps = ExifFix.CreateMetadata(comps, thumbnail);
            var result = JpgScan.AssembleJpg(comps);
            var signed = SignJpg(result);
            return signed;            
        }


        public byte[] SignJpg(byte[] input)
        {
            DSiContext ctr_ctx = new DSiContext();
            DSiContext ccm_ctx = new DSiContext();
            ctr_ctx.Initialize();
            ccm_ctx.Initialize();
            unsafe
            {
                Debug.WriteLine($"IV = {IV.Select(_ => $"{_:X2}").JoinToString(" ")}");
                fixed (byte* nonce = &IV[0])
                {
                    uint size = (uint)input.Length;
                    Debug.WriteLine($"Size={size:X6}");
                    uint total_size = (size + 0xF) & 0xFFFFFFF0;
                    Debug.WriteLine($"Total size={total_size:X6}");
                    var _in_buf = new byte[total_size];
                    Array.Copy(input, _in_buf, size);

                    var _block = new byte[16];

                    fixed (byte* in_buf = &_in_buf[0])
                    fixed (byte* block = &_block[0])
                    fixed (byte* key = &Key[0])
                    {
                        Memset(&in_buf[0x18A], 0, 0x1C);
                        Memset(block, 0, 16);

                        Debug.WriteLine($"Block1: {_block.Select(_ => $"{_:X2}").JoinToString(" ")}");

                        ctr_ctx.InitCtr(key, block);

                        Debug.WriteLine($"Block2: {_block.Select(_ => $"{_:X2}").JoinToString(" ")}");

                        ctr_ctx.CryptCtrBlock(block, block);

                        Debug.WriteLine($"Block3: {_block.Select(_ => $"{_:X2}").JoinToString(" ")}");

                        WeirdFunc((uint*)block);

                        Debug.WriteLine($"Block4: {_block.Select(_ => $"{_:X2}").JoinToString(" ")}");

                        var final_bytes = ((size - 1) & 0xF) + 1;
                        if (final_bytes == 0x10)
                        {
                            XorBlock((uint*)block, (uint*)&in_buf[size - final_bytes]);
                        }
                        else
                        {
                            var _tmp_block = new byte[16];
                            fixed (byte* tmp_block = &_tmp_block[0])
                            {
                                Memset(tmp_block, 0, 16);
                                Memcpy(&tmp_block[16 - final_bytes], &in_buf[size - final_bytes], final_bytes);
                                tmp_block[15 - final_bytes] = 0x80;
                                Debug.WriteLine($"tmp: {_block.Select(_ => $"{_:X2}").JoinToString(" ")}");
                                WeirdFunc((uint*)block);
                                Debug.WriteLine($"tmp: {_block.Select(_ => $"{_:X2}").JoinToString(" ")}");
                                XorBlock((uint*)block, (uint*)tmp_block);
                            }                            
                        }
                        Debug.WriteLine($"finalb={final_bytes}");
                        Debug.WriteLine($"tmp: {_block.Select(_ => $"{_:X2}").JoinToString(" ")}");
                        Memcpy(&in_buf[size - final_bytes], block, 16);
                        ccm_ctx.InitCcm(key, 16, 0, total_size, nonce);

                        var _out_buf = new byte[total_size];
                        var _mac = new byte[16];
                        fixed (byte* out_buf = &_out_buf[0])
                        fixed (byte* mac = &_mac[0])
                        {
                            Memset(mac, 0, 16);
                            ccm_ctx.EncryptCcm(in_buf, out_buf, total_size, mac);

                            Debug.WriteLine($"MAC = {_mac.Select(_ => $"{_:X2}").JoinToString(" ")}");

                            Array.Copy(input, _in_buf, size);

                            Memcpy(&in_buf[0x18A], nonce, 0xC);
                            Memcpy(&in_buf[0x196], mac, 0x10);

                            return _in_buf.Take((int)size).ToArray();
                        }
                    } // fixed
                } // fixed
            } // unsafe
        }

        private byte[] GenerateNonce()
        {
            var rand = new Random();
            byte[] nonce = new byte[12];
            rand.NextBytes(nonce);
            for (int i = 0; i < 12; i++)
                if (i % 4 == 2 || i % 4 == 3) nonce[i] = 0; 
            return nonce;
        }

        static unsafe void WeirdFunc(uint* block)
        {
            uint tmp = block[3];
            block[3] = (uint)(*((ulong*)block + 1) >> 31);
            block[2] = (uint)(*(ulong*)(&block[1]) >> 31);
            block[1] = (uint)(*(ulong*)block >> 31);
            block[0] *= 2;
            if ((tmp >> 31) != 0)
                block[0] ^= 0x87;
        }

        static unsafe void XorBlock(uint* block, uint* xor_block)
        {
            for (int i = 0; i < 4; i++) block[i] ^= xor_block[i];
        }
    }
}
