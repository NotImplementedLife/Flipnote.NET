using FlipnoteDotNet.App.Canvas.Components;
using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.App.Service;
using FlipnoteDotNet.Canvas;
using FlipnoteDotNet.PropertyEditor;
using FlipnoteDotNet.Utils.GUI;
using System.Diagnostics;

namespace FlipnoteDotNet.App.Controls
{
    public partial class FlipnoteEditorControl
    {
        private FlipnoteEditorService FlipnoteEditorService;

        private void InitializeFrames(FlipnoteEditorService flipnoteEditorService)
        {
            FlipnoteEditorService = flipnoteEditorService;
            CanvasControl.DragEnter += CanvasControl_DragEnter;
            CanvasControl.DragDrop += CanvasControl_DragDrop;
            CanvasControl.ComponentTransformChanged += CanvasControl_ComponentTransformChanged;
            CanvasControl.SelectionChanged += CanvasControl_SelectionChanged;

            PropertyEditor.ByUserValueChangedNotPreview += PropertyEditor_ByUserValueChangedNotPreview;
        }

        private void PropertyEditor_ByUserValueChangedNotPreview(object sender, ByUserValueChangedEventArgs e)
        {
            //Debug.WriteLine($"FinalValueChanged: {e.Name} => {e.Getter.Invoke(PropertyEditor.Object, null)}");
            FlipnoteEditorService.ChangeProperty(PropertyEditor.Object, e.OldValue, e.NewValue, e.PropertyData.Value);
        }

        private void CanvasControl_SelectionChanged(object sender, EventArgs e)
        {
            PropertyEditor.Object = CanvasControl.SelectedComponent;
        }

        private void CanvasControl_ComponentTransformChanged(object sender, FlipnoteDotNet.Canvas.Components.CanvasComponent component, CanvasTransform oldTransform, CanvasTransform newTransform)
        {
            Debug.WriteLine("TransformChanged");

            if (component is not FlipnoteCanvasComponent flipnoteComponent)
                throw new InvalidOperationException("component is not FlipnoteCanvasComponent");
            FlipnoteEditorService.ChangeComponentTransformOnCurrentFrame(flipnoteComponent, oldTransform, newTransform);
        }

        private void CanvasControl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DragDropObject<Asset>)))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void CanvasControl_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DragDropObject<Asset>)))
            {
                var asset = (e.Data.GetData(typeof(DragDropObject<Asset>)) as DragDropObject<Asset>).Value;
                FlipnoteEditorService.AddAssetToCurrentFrame(asset);         
            }
        }

    }
}
