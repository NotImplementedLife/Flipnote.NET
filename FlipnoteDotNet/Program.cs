using FlipnoteDotNet.App;
using FlipnoteDotNet.App.Canvas.Components;
using FlipnoteDotNet.App.Editors;
using FlipnoteDotNet.App.Forms;
using FlipnoteDotNet.App.Menus;
using FlipnoteDotNet.Canvas;
using FlipnoteDotNet.Canvas.Components;
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

            //MenuStripLoader.Load(typeof(FlipnoteEditorForm));
            //Environment.Exit(0);

            WarmUp();
            //Application.Run(new TestForm());
            var form = MenuProvider.PrepareForm(new FlipnoteEditorForm(AppState.Instance),
                (f, m) =>
                {
                    f.MainMenuStrip.Items.Clear();
                    f.MainMenuStrip.Items.AddRange(m);
                });

            Application.Run(form);
        }

        static void WarmUp()
        {
            InitializeStaticClass(typeof(FlipnoteDotNet.App.Config));
            InitializeStaticClass(typeof(FlipnoteDotNet.Utils.MainThreadInvoker));
            InitializeStaticClass(typeof(FlipnoteDotNet.PropertyEditor.PropertyEditors));
            MethodPreparer.WarmUp(typeof(CanvasModel), "Render");
            MethodPreparer.WarmUp(typeof(CanvasModel), "Update");                        

            var ignoreAttr = new PropertyEditorAttribute(ignore: true);
            PropertiesCollection.RegisterAttribute<CanvasComponent>(nameof(CanvasComponent.DirectTransformValues), ignoreAttr);
            PropertiesCollection.RegisterAttribute<CanvasComponent>(nameof(CanvasComponent.IgnoreGraphicsTransform), ignoreAttr);
            PropertiesCollection.RegisterAttribute<CanvasComponent>(nameof(CanvasComponent.InverseTransformValues), ignoreAttr);
            PropertiesCollection.RegisterAttribute<FlipnoteSprite>(nameof(FlipnoteSprite.Source), ignoreAttr);

            PropertyEditor.PropertyEditors.RegisterDefaultEditor<CanvasTransform, CanvasTransformEditor>();
        }

        private static void InitializeStaticClass(Type t) => RuntimeHelpers.RunClassConstructor(t.TypeHandle);
    }
}