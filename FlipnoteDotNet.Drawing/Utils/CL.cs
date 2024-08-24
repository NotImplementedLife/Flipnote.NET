using Cloo;
using System.Diagnostics;

namespace FlipnoteDotNet.Drawing.Utils
{
    public static class CL
    {
        private static ComputePlatform Platform;
        public static ComputeContext Context;                

        private static bool IsInited = false;

        public static ComputeMemoryFlags ReadWriteHostFlags = ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.UseHostPointer;
        public static ComputeMemoryFlags ReadOnlyHostFlags = ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.UseHostPointer;

        public static void Init()
        {            
            if (IsInited) return;
            IsInited = true;
            Debug.WriteLine("Getting Platform");
            Platform = ComputePlatform.Platforms[0];
            Debug.WriteLine($"Platform = {Platform.Name}");
            Debug.WriteLine("Context");
            Context = new ComputeContext(ComputeDeviceTypes.Gpu, new ComputeContextPropertyList(Platform), null, IntPtr.Zero);
            Debug.WriteLine("Program");

            CLOrderedDithering.Init();
        }

        public static void TryBuild(ComputeProgram program, string buildFlags)
        {
            try
            {
                program.Build(null, buildFlags, null, IntPtr.Zero);
            }
            catch
            {
                string buildLog = program.GetBuildLog(CL.Context.Devices[0]);
                Debug.WriteLine("\n********** Build Log **********\n" + buildLog + "\n*************************");
                throw;
            }
        }

    }
}
