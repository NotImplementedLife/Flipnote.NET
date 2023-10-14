using System;
using static PPMLib.Utils.Memory;

namespace PPMLib.IO.JPEG
{
    public unsafe struct DSiEsContext
    {
        private fixed byte _Key[16];
        private fixed byte _Nonce[12];
        private int RandomNonce;

        private byte* Key;
        private byte* Nonce;

        public void Initialize(byte* key)
        {
            fixed (byte* _ = &_Key[0]) Key = _;
            fixed (byte* _ = &_Nonce[0]) Nonce = _;

            Memcpy(Key, key, 16);
            RandomNonce = 1;
        }

        public void SetNonce(byte* nonce)
        {
            Memcpy(Nonce, nonce, 12);
            RandomNonce = 0;
        }

        public void SetRandomNonce() => RandomNonce = 1;

        public int Decrypt(byte* buffer, byte* metablock, uint size)
        {
            byte[] _ctr=new byte[16];
            byte[] _nonce = new byte[12];
            byte[] _scratchpad = new byte[16];
            byte[] _chkmac = new byte[16];
            byte[] _genmac = new byte[16];

            fixed (byte* ctr = &_ctr[0])
            fixed (byte* nonce = &_nonce[0])
            fixed (byte* scratchpad = &_scratchpad[0])
            fixed (byte* chkmac = &_chkmac[0])
            fixed (byte* genmac = &_genmac[0])
            {

                DSiContext cryptoctx = new DSiContext();
                cryptoctx.Initialize();
                uint chksize;

                Memcpy(chkmac, metablock, 16);

                Memcpy(ctr, metablock + 16, 16);
                ctr[0] = 0;
                ctr[13] = 0;
                ctr[14] = 0;
                ctr[15] = 0;

                cryptoctx.InitCtr(Key, ctr);
                cryptoctx.CryptCtrBlock(metablock + 16, scratchpad);                              

                chksize = (uint)((scratchpad[13] << 16) | (scratchpad[14] << 8) | (scratchpad[15] << 0));

                if (scratchpad[0] != 0x3A || chksize != size)
                    return -1;

                Memcpy(nonce, metablock + 17, 12);

                cryptoctx.InitCcm(Key, 16, size, 0, nonce);
                cryptoctx.DecryptCcm(buffer, buffer, size, genmac);

                return Memcmp(genmac, chkmac, 16) != 0 ? -1 : 0;                
            }
        }

        public void Encrypt(byte* buffer, byte* metablock, uint size)
        {
            int i;

            byte[] _ctr = new byte[16];
            byte[] _nonce = new byte[12];
            byte[] _scratchpad = new byte[16];
            byte[] _mac = new byte[16];            

            fixed (byte* ctr = &_ctr[0])
            fixed (byte* nonce = &_nonce[0])
            fixed (byte* scratchpad = &_scratchpad[0])
            fixed (byte* mac = &_mac[0])
            {
                DSiContext cryptoctx = new DSiContext();
                cryptoctx.Initialize();

                if (RandomNonce != 0) 
                {
                    var rand = new Random();                    
                    for (i = 0; i < 12; i++)
                        nonce[i] = (byte)rand.Next();
                }
                else
                {
                    Memcpy(nonce, Nonce, 12);
                }

                cryptoctx.InitCcm(Key, 16, size, 0, nonce);
                cryptoctx.EncryptCcm(buffer, buffer, size, mac);                

                Memset(scratchpad, 0, 16);
                scratchpad[0] = 0x3A;
                scratchpad[13] = (byte)(size >> 16);
                scratchpad[14] = (byte)(size >> 8);
                scratchpad[15] = (byte)(size >> 0);

                Memset(ctr, 0, 16);
                Memcpy(ctr + 1, nonce, 12);

                cryptoctx.InitCtr(Key, ctr);

                cryptoctx.CryptCtrBlock(scratchpad, metablock + 16);
                Memcpy(metablock + 17, nonce, 12);
                Memcpy(metablock, mac, 16);
            }            
        }
    }
}
