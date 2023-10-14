namespace PPMLib.Utils
{
    internal unsafe class Memory
    {
        public static void Memcpy(byte* d, byte* s, uint len)
        {
            for (int i = 0; i < len; i++) *(d++) = *(s++);
        }

        public static void Memset(byte* b, byte x, uint len)
        {
            for (int i = 0; i < len; i++) *(b++) = x;
        }

        public static int Memcmp(byte* str1, byte* str2, uint n)
        {
            for (int i = 0; i < n; i++) 
            {
                int d = *(str1++) - *(str2++);
                if (d != 0) return d;
            }
            return 0;
        }
    }
}
