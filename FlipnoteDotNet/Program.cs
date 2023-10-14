using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.Utils.Manipulator;
using FlipnoteDotNet.Utils.ProjectBinary;
using FlipnoteDotNet.Utils.Serialization;
using FlipnoteDotNet.Utils.Temporal;
using FlipnoteDotNet.Utils.Temporal.ValueTransformers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace FlipnoteDotNet
{
    internal static class Program
    {        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Constants.Init();
            Manipulators.Initialize();
            LayerBytes.Initialize();
            SerializationContext.Initialize();

            /*var w = new SerializeWriter();

            var x = new SimpleValueTransformer<int>(new ConstantValueChanger<int>(5));

            w.Write(x);

            var b = w.ToArray();
            Debug.WriteLine(b.Select(_ => $"_:X2").JoinToString(" "));

            var r = new SerializeReader(b);

            var y = r.Read<SimpleValueTransformer<int>>();
            Debug.WriteLine(y);

            return;*/
            Application.Run(new MainForm());
        }
    }
}