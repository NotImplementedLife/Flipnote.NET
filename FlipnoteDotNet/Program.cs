using FlipnoteDotNet.App;
using FlipnoteDotNet.App.Editors;
using FlipnoteDotNet.App.Forms;
using FlipnoteDotNet.Canvas;
using FlipnoteDotNet.Core;
using FlipnoteDotNet.PropertyEditor;
using System.Runtime.CompilerServices;

namespace FlipnoteDotNet
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {            
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.            
            ApplicationConfiguration.Initialize();
            WarmUp();
            //Application.Run(new TestForm());
            Application.Run(new FlipnoteEditorForm(AppState.Instance)); 
        }

        static void WarmUp()
        {
            InitializeStaticClass(typeof(FlipnoteDotNet.App.Config));
            InitializeStaticClass(typeof(FlipnoteDotNet.Utils.MainThreadInvoker));
            InitializeStaticClass(typeof(FlipnoteDotNet.PropertyEditor.PropertyEditors));
            MethodPreparer.WarmUp(typeof(CanvasModel), "Render");
            MethodPreparer.WarmUp(typeof(CanvasModel), "Update");            

            PropertyEditor.PropertyEditors.RegisterDefaultEditor<CanvasTransform, CanvasTransformEditor>();
        }

        private static void InitializeStaticClass(Type t)
        {
            RuntimeHelpers.RunClassConstructor(t.TypeHandle);
        }
    }
}