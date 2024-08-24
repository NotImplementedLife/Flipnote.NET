namespace FlipnoteDotNet.Drawing.Renderers
{
    public static class BayerMatrices
    {
        public static readonly byte[] M0 = new byte[] 
        { 
            0 
        };

        public static readonly byte[] M1 = new byte[] 
        { 
            0, 2, 
            3, 1 
        };

        public static readonly byte[] M2 = new byte[]
        {
            0, 8, 2, 10,
            12, 4, 14, 6,
            3, 11, 1, 9,
            15, 7, 13, 5
        };

        public static readonly byte[] M3 = new byte[]
        {
            8, 32, 8, 40, 2, 34, 10, 42,
            48, 16, 56, 24, 50, 18, 58, 26,
            12, 44, 4, 36, 14, 46, 6, 38,
            60, 28, 52, 20, 62, 30, 54, 22,
            3, 35, 11, 43, 1, 33, 9, 41,
            51, 19, 59, 27, 49, 17, 57, 25,
            15, 47, 7, 39, 13, 45, 5, 37,
            63, 31, 55, 23, 61, 29, 53, 21
        };



        public static readonly byte[][] M = new byte[][] { M0, M1, M2, M3 };
    }
}
