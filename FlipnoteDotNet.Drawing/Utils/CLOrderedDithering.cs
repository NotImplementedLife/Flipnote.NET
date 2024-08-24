using Cloo;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FlipnoteDotNet.Drawing.Utils
{
    public static class CLOrderedDithering
    {
        private static string KernelCode = Properties.Resources.CLKernel_OrderedDithering;

        private static ComputeProgram Program;

        private static string ProgramBuildFlags = "-cl-fast-relaxed-math -cl-no-signed-zeros";
        private static ComputeKernel Kernel;
        private static ComputeCommandQueue Queue;
        public static void Init()
        {
            Program = new ComputeProgram(CL.Context, KernelCode);
            CL.TryBuild(Program, ProgramBuildFlags);            
            Kernel = Program.CreateKernel("CLKernel_OrderedDithering");
            Queue = new ComputeCommandQueue(CL.Context, CL.Context.Devices[0], ComputeCommandQueueFlags.None);            
        }

        public static void Call(int[] pixels, int width, int height, byte[] m, int order, Palette palette, int alphaThreshold)
        {            
            int paletteLength = palette.Colors.Length;
            int[] colors = new int[palette.Colors.Length];
            for (int i = 0; i < paletteLength; i++) colors[i] = palette.Colors[i].ToArgb();

            int n = 1 << order;
            int modN = n - 1;
            int n2over2 = n * n / 2;

            using (var pxBuffer = new ComputeBuffer<int>(CL.Context, CL.ReadWriteHostFlags, pixels))
            using (var mBuffer = new ComputeBuffer<byte>(CL.Context, CL.ReadOnlyHostFlags, m))
            using (var palBuffer = new ComputeBuffer<int>(CL.Context, CL.ReadOnlyHostFlags, colors))            
            {                
                Kernel.SetMemoryArgument(0, pxBuffer);
                Kernel.SetValueArgument(1, width);                

                Kernel.SetMemoryArgument(2, mBuffer);
                Kernel.SetValueArgument(3, order);

                Kernel.SetValueArgument(4, palette.SpreadR);
                Kernel.SetValueArgument(5, palette.SpreadG);
                Kernel.SetValueArgument(6, palette.SpreadB);

                Kernel.SetMemoryArgument(7, palBuffer);
                Kernel.SetValueArgument(8, paletteLength);
                Kernel.SetValueArgument(9, alphaThreshold);

                Kernel.SetValueArgument(10, modN);
                Kernel.SetValueArgument(11, n2over2);

                Queue.Execute(Kernel, new long[] { 0, 0 }, new long[] { height, width }, null, null);
                
                Queue.Flush();
                Queue.Finish();

                GCHandle arrCHandle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
                Queue.Read(pxBuffer, true, 0, width * height, arrCHandle.AddrOfPinnedObject(), null);
                arrCHandle.Free();
            }
        }

    }
}
