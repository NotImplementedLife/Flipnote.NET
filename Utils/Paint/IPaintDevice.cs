using FlipnoteDotNet.Utils.Paint.Tools;

namespace FlipnoteDotNet.Utils.Paint
{
    public interface IPaintDevice
    {
        int GetPixel(int x, int y);
        void SetPixel(int x, int y, int pixel);
        void StartPreview();
        void PushPreview();
        void DiscardPreview();
        void UpdateDevice();

        PreviewPoint CreatePreviewPoint(int x, int y);
        void PreviewLine(PreviewPoint p1, PreviewPoint p2);
        void AttachOperation(IPaintOperation operation);

    }
}
