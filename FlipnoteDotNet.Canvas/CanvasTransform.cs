using System.Drawing.Drawing2D;
using System.Numerics;

namespace FlipnoteDotNet.Canvas
{
    public record struct CanvasTransform(
        int X = 0,
        int Y = 0,
        Angle Rotation = default,
        float ScaleX = 1,
        float ScaleY = 1,
        float AnchorX = 0.5f,
        float AnchorY = 0.5f
        )
    {
        public CanvasTransform() : this(0, 0, 0, 1, 1, 0.5f, 0.5f) {}       

        public readonly Matrix3x2 CreateDirectTransform(int width, int height)
        {            
            var anchorX = (float)(AnchorX * width);
            var anchorY = (float)(AnchorY * height);
            var c = Rotation.CosValue;
            var s = Rotation.SinValue;
            var taX = X + anchorX;
            var taY = Y + anchorY;

            var m11 = c * ScaleX;
            var m12 = -s * ScaleY;
            var m21 = s * ScaleX;
            var m22 = c * ScaleY;
            var x = taX - anchorX * m11 - anchorY * m12;
            var y = taY - anchorX * m21 - anchorY * m22;

            return new Matrix3x2(m11, m21, m12, m22, x, y);
        }

        public readonly Matrix3x2 CreateInverseTransform(int width, int height)
        {
            var anchorX = (float)(AnchorX * width);
            var anchorY = (float)(AnchorY * height);
            var c = Rotation.CosValue;
            var s = Rotation.SinValue;

            var order = MatrixOrder.Append;
            var m = new Matrix();

            m.Multiply(new Matrix(1, 0, 0, 1, -X - anchorX, -Y - anchorY), order);
            m.Multiply(new Matrix(c, -s, s, c, 0, 0), order);
            m.Multiply(new Matrix(1 / ScaleX, 0, 0, 1 / ScaleY, 0, 0), order);            
            m.Multiply(new Matrix(1, 0, 0, 1, anchorX, anchorY), order);

            return m.MatrixElements;
        }
    }
}
