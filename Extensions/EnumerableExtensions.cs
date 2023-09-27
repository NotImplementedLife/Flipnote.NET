﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Extensions
{
    internal static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }

        public static void ForEach<T, __Discarded>(this IEnumerable<T> items, Func<T, __Discarded> action)
        {
            foreach (var item in items)
                action(item);
        }

        public static IEnumerable<T> Peek<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items) 
            {
                action(item);
                yield return item;
            }
        }        

        public static Bitmap ToBitmap32bppPArgb(this uint[] data, int width, int height)
        {
            unsafe
            {
                fixed (uint* ptr = data)
                    return new Bitmap(width, height, 4 * width, PixelFormat.Format32bppPArgb, new IntPtr(ptr));
            }
        }

        public static Bitmap ToBitmap32bppPArgb(this int[] data, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            Marshal.Copy(data, 0, bmpData.Scan0, width * height);
            bmp.UnlockBits(bmpData);
            return bmp;            
        }

        public static string JoinToString<T>(this IEnumerable<T> items, string separator = "\n")
            => string.Join(separator, items.Select(_ => _?.ToString()));


    }
}
