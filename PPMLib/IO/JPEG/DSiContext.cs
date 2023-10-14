using PPMLib.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using static PPMLib.Utils.Memory;

namespace PPMLib.IO.JPEG
{
    public unsafe struct DSiContext
    {
        private fixed byte _Ctr[16];
        private fixed byte _Mac[16];
        private fixed byte _S0[16];

        private byte* Ctr;
        private byte* Mac;
        private byte* S0;

        private uint MacLen;

        private byte[] Key;        

        public void Initialize()
        {
            fixed (byte* _ = &_Ctr[0]) Ctr = _;            
            fixed (byte* _ = &_Mac[0]) Mac = _;            
            fixed (byte* _ = &_S0[0]) S0 = _;            
        }

        public void SetKey(byte* key)
        {
            var keyswap = new byte[16];
            for (int i = 0; i < 16; i++) keyswap[i] = key[15 - i];
            
            Key = keyswap;            

            //Debug.WriteLine($"setKey: {BytesToString(key, 16)}");
            //Debug.WriteLine($"setKSW: {keyswap.Select(_ => $"{_:X2}").JoinToString(" ")}");                       
        }

        public void AddCtr(byte carry)
        {                        
            byte sum;
            for (int i = 15; i >= 0; i--)
            {
                sum = (byte)(Ctr[i] + carry);
                carry = (byte)(sum < Ctr[i] ? 1 : 0);
                Ctr[i] = sum;
            }
            //Debug.WriteLine($"dsiAddCtr: {BytesToString(Ctr, 16)}");
        }

        private string BytesToString(byte* bytes, int count)
        {
            if (bytes == null) return "null";
            var b = new byte[count];
            for (int i = 0; i < count; i++) b[i] = bytes[i];
            return b.Select(_ => $"{_:X2}").JoinToString(" ");
        }

        private string BytesToString(byte[] bytes)
        {            
            return bytes.Select(_ => $"{_:X2}").JoinToString(" ");
        }

        public void SetCtr(byte* ctr)
        {
            for (int i = 0; i < 16; i++) Ctr[i] = ctr[15 - i];
            //Debug.WriteLine($"dsiSetCtr: {BytesToString(Ctr, 16)}");
        }

        public void InitCtr(byte* key, byte* ctr)
        {
            SetKey(key);
            SetCtr(ctr);
        }

        static byte[] Encrypt(byte[] bytes, byte[] key, byte[] iv)
        {
            using (AesManaged aes = new AesManaged())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(key, iv), CryptoStreamMode.Write))
                    {
                        cs.Write(bytes, 0, bytes.Length);
                        return ms.ToArray();
                    }
                }
            }
        }

        private void AesCryptEcb(byte* input, byte* output)
        {            
            var _input = new byte[16];
            for (int i = 0; i < 16; i++) _input[i] = input[i];
            var _output = Encrypt(_input, Key, Enumerable.Repeat((byte)0, 16).ToArray());
            for (int i = 0; i < 16; i++) output[i] = _output[i];            
        }

        public void CryptCtrBlock(byte* input, byte* output)
        {            
            var stream = new byte[16];
            fixed (byte* pstream = &stream[0]) AesCryptEcb(Ctr, pstream);

            //Debug.WriteLine("__CCBi: " + BytesToString(input, 16));
            //Debug.WriteLine("__CCBs: " + BytesToString(stream));

            if (input!=null)
            {
                //Debug.WriteLine("input!=null");
                for (int i = 0; i < 16; i++)
                    output[i] = (byte)(stream[15 - i] ^ input[i]);  
            }
            else
            {
                //Debug.WriteLine("input==null");
                for (int i = 0; i < 16; i++)
                    output[i] = stream[15 - i];
            }
            AddCtr(1);            
        }

        public void InitCcm(byte* key, uint maclength, uint payloadlength, uint assoclength, byte* nonce)
        {
            int i;
            SetKey(key);
            MacLen = maclength;

            maclength = (maclength - 2) / 2;
            payloadlength = (uint)((payloadlength + 15) & ~15);

            // CCM B0 block:
            // [1-byte flags] [12-byte nonce] [3-byte size]
            Mac[0] = (byte)((maclength << 3) | 2);
            if (assoclength != 0) 
                Mac[0] |= (1 << 6);
            for (i = 0; i < 12; i++)
                Mac[1 + i] = nonce[11 - i];
            Mac[13] = (byte)(payloadlength >> 16);
            Mac[14] = (byte)(payloadlength >> 8);
            Mac[15] = (byte)(payloadlength >> 0);

            AesCryptEcb(Mac, Mac);            

            // CCM CTR:
            // [1-byte flags] [12-byte nonce] [3-byte ctr]
            Ctr[0] = 2;
            for (i = 0; i < 12; i++)
                Ctr[1 + i] = nonce[11 - i];
            Ctr[13] = Ctr[14] = Ctr[15] = 0;


            CryptCtrBlock(null, S0);            
        }

        public void EncryptCcmBlock(byte* input, byte* output, byte* mac)
        {            
            for (int i = 0; i < 16; i++)
                Mac[i] ^= input[15 - i];

            AesCryptEcb(Mac, Mac);            

            if (mac != null) 
            {
                for (int i = 0; i < 16; i++)
                    mac[i] = (byte)(Mac[15 - i] ^ S0[i]);
            }

            if (output != null)
                CryptCtrBlock(input, output);                
        }

        public void DecryptCcmBlock(byte* input, byte* output, byte* mac)
        {
            int i;
            if (output!=null)
            {
                CryptCtrBlock(input, output);                

                for (i = 0; i < 16; i++)
                    Mac[i] ^= output[15 - i];
            }
            else
            {
                for (i = 0; i < 16; i++)
                    Mac[i] ^= input[15 - i];
            }

            AesCryptEcb(Mac, Mac);            


            if (mac!=null)
            {
                for (i = 0; i < 16; i++)
                    mac[i] = (byte)(Mac[15 - i] ^ S0[i]);
            }

        }

        public void DecryptCcm(byte* input, byte* output, uint size, byte* mac)
        {
            byte[] ablock=new byte[16];
            byte[] actr=new byte[16];

            fixed (byte* block = &ablock[0])
            fixed (byte* ctr = &actr[0])
            {

                while (size > 16)
                {
                    DecryptCcmBlock(input, output, mac);
                    if (input != null)  input += 16;
                    if (output != null) output += 16;
                    size -= 16;
                }

                Memcpy(ctr, Ctr, 16);
                Memset(block, 0, 16);
                CryptCtrBlock(block, block);                
                Memcpy(Ctr, ctr, 16);
                Memcpy(block, input, size);

                DecryptCcmBlock(block, block, mac);                
                Memcpy(output, block, size);
            }
        }

        public void EncryptCcm(byte* input, byte* output, uint size, byte* mac)
        {
            var ablock = new byte[16];

            while (size > 16)
            {
                EncryptCcmBlock(input, output, mac);
                if (input != null) input += 16;
                if (output != null) output += 16;
                size -= 16;
            }

            //Debug.WriteLine("Kah");

            fixed (byte* block = &ablock[0]) 
            {
                Memset(block, 0, 16);
                Memcpy(block, input, size);
                //Debug.WriteLine($"block: {BytesToString(block, 16)}");
                EncryptCcmBlock(block, block, mac);
                Memcpy(output, block, size);
            }
        }      

        
    }
}
