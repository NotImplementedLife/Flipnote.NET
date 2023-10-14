using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.Utils;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace FlipnoteDotNet
{
    internal static class AppConfig
    {
        private static IniFile CreateIniFile()
        {
            var ini = new IniFile();
            //if (!ini.KeyExists("...")) ini.Write("...", ...);         
            return ini;
        }
        public static readonly IniFile IniFile = CreateIniFile();

        public static byte[] DSiJpegAesKey
        {
            get => IniFile.TryReadKey(Key_DsiJpegAesKey, out var strKey)
                ? Regex.IsMatch(strKey, "^[0-9A-Fa-f]{32}$")
                ? HexStringToByteArray(strKey) : null : null;
            set => IniFile.Write(Key_DsiJpegAesKey, (value ?? new byte[0]).Select(_ => $"{_:x2}").JoinToString(""));
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            int GetHexVal(char val) => val - (val < 58 ? 48 : (val < 97 ? 55 : 87));

            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");
            byte[] arr = new byte[hex.Length >> 1];
            for (int i = 0; i < hex.Length >> 1; ++i)            
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));            
            return arr;
        }


        public static readonly string Key_DsiJpegAesKey = "dsi_jpeg_aes_key";
    }
}
