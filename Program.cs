using FlipnoteDotNet.Data.Layers;
using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.Utils;
using FlipnoteDotNet.Utils.Temporal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlipnoteDotNet
{
    internal static class Program
    {
        class A : AbstractTransformableTemporalContext
        {
            public A()
            {
                X = new TimeDependentValue<int>(this);
                this.Initialize();
            }

            public TimeDependentValue<int> X { get; }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Constants.Reflection.Init();

            /*var a = new A();

            a.X.PutTransformer(new ConstantValueTransformer<int>(16), 2);
            a.X.PutTransformer(new ConstantValueTransformer<int>(23, false), 10);
            a.X.UpdateTransformations();
            a.X.UpdateTimestamps();

            a.X.GetTransformers().ForEach(_ => Debug.WriteLine(_));

            for(int i=0;i<20;i++)
            {
                a.CurrentTimestamp = i;
                Debug.WriteLine($"{i} : {a.X.CurrentValue}");
            }
            
            return;*/

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
