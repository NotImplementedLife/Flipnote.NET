using FlipnoteDotNet.Canvas.Selections;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Numerics;

namespace FlipnoteDotNet.Canvas.Components
{
    public abstract class CanvasComponent : INotifyPropertyChanged
    {        
        public readonly int Width;
        public readonly int Height;
        public readonly bool IgnoreTransform;
        public bool IgnoreGraphicsTransform { get; protected set; } = false;

        private string fName = null;
        public string Name
        {
            get => fName;
            set { fName = value; OnPropertyChanged(new PropertyChangedEventArgs(nameof(Name))); }
        }

        protected CanvasComponent(string name, int width, int height, bool ignoreTransform, bool ignoreGraphicsTransform = false)
        {
            Name = name;
            Width = width;
            Height = height;
            IgnoreTransform = ignoreTransform;
            IgnoreGraphicsTransform = ignoreTransform || ignoreGraphicsTransform;     
        }

        private CanvasTransform fTransform = new CanvasTransform();
        internal Matrix DirectTransform { get; private set; } = new Matrix();
        internal Matrix InverseTransform { get; private set; } = new Matrix();

        private Matrix3x2 fDirectTransformValues = new Matrix3x2(1, 0, 0, 1, 0, 0);
        private Matrix3x2 fInverseTransformValues = new Matrix3x2(1, 0, 0, 1, 0, 0);

        public Matrix3x2 DirectTransformValues => fDirectTransformValues;
        public Matrix3x2 InverseTransformValues => fInverseTransformValues;

        internal CanvasProcessor.DirtyLock DirtyLock;

        internal Matrix CloneDirectTransform()
        {
            return new Matrix(fDirectTransformValues);
        }

        public CanvasTransform Transform
        {
            get => fTransform;
            set
            {
                fTransform = value;

                fDirectTransformValues = fTransform.CreateDirectTransform(Width, Height);
                DirectTransform?.Dispose();
                DirectTransform = new Matrix(fDirectTransformValues);

                fInverseTransformValues = fTransform.CreateInverseTransform(Width, Height);
                InverseTransform?.Dispose();
                InverseTransform = new Matrix(fInverseTransformValues);

                OnTransformChanged(EventArgs.Empty);
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Transform)));
            }
        }

        protected virtual void OnTransformChanged(EventArgs e)
        {
            TransformChanged?.Invoke(this, e);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        public event EventHandler TransformChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public void Invalidate()
        {
            DirtyLock.NotifyDirty();
        }

        public abstract void Draw(Graphics g);

        public virtual void Update() { }

        public bool HitTest(int hitX, int hitY, out float relX, out float relY)
        {
            var points = new[] { new PointF(hitX, hitY) };
            if (!IgnoreTransform)
                InverseTransform.TransformPoints(points);
            return OnHitTest(points[0].X, points[0].Y, out relX, out relY);
        }

        protected virtual bool OnHitTest(float hitX, float hitY, out float relX, out float relY)
        {
            relX = Width > 0 ? 1f * hitX / Width : 0;
            relY = Height > 0 ? 1f * hitY / Height : 0;
            return 0 <= hitX && hitX < Width && 0 <= hitY && hitY < Height;
        }

        public virtual SelectionState CreateSelectionState()
        {
            return new BoundingBoxSelectionState(this);
        }

        public override string ToString() => $"{GetType().Name}{{Name={Name}}}";
        
        public abstract CanvasComponent Clone();

    }
}
