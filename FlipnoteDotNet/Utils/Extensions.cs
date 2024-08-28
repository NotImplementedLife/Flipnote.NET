using System;
using System.Collections.Generic;
using System.Linq;
namespace FlipnoteDotNet.Utils
{
    public static class Extensions
    {
        public static int[] CloneArray(this int[] arr)
        {            
            var clone = new int[arr.Length];
            Array.Copy(arr, clone, arr.Length);
            return clone;
        }

    }
}
