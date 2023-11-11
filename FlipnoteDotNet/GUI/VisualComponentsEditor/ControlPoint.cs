using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.VisualComponentsEditor
{
    public class ControlPoint
    {
        protected readonly VisualComponent Owner;
        // position inside Owner bounds scaled to [0,1]
        protected float X, Y;

        public ControlPoint(VisualComponent owner, float x, float y)
        {
            Owner = owner;
            X = x;
            Y = y;
        }

        public Point GetRealPosition()
        {
            var p = new[] { new Point(Owner.Location.X + (int)(Owner.Size.Width * X), Owner.Location.Y + (int)(Owner.Size.Height * Y)) };
            Owner.Transform.TransformPoints(p);
            return p[0];
        }

        public virtual void OnMoveStarted(Point position) { }
        public virtual void OnMoved(Point newPosition) { }
        public virtual void OnMoveDone() { }
    }

    public class ResizePoint : ControlPoint
    {
        public ResizePoint(VisualComponent owner, float x, float y) : base(owner, x, y) { }


        Matrix InvTransform;
        Point[] StartPos = new Point[1];        

        Point P0, P1;        


        public override void OnMoveStarted(Point position) 
        {
            InvTransform = Owner.Transform.Clone();
            InvTransform.Invert();
            StartPos[0] = position;
            InvTransform.TransformPoints(StartPos);

            P0 = Owner.Location;
            P1 = Owner.Location;
            P1.Offset(Owner.Size.Width, Owner.Size.Height);
        }
        public override void OnMoved(Point newPosition) 
        {            
            Point[] MovePos = new[] { newPosition };            
            InvTransform.TransformPoints(MovePos);

            Debug.WriteLine($"M={StartPos[0]} {MovePos[0]}");

            var dx = MovePos[0].X - StartPos[0].X;
            var dy = MovePos[0].Y - StartPos[0].Y;

            var Q0 = new Point(P0.X + (int)((1 - X) * dx), P0.Y + (int)((1 - Y) * dy));
            var Q1 = new Point(P1.X + (int)(X * dx), P1.Y + (int)(Y * dy));

            Owner.Location = Q0;
            Owner.Size = new Size(Q1.X - Q0.X, Q1.Y - Q0.Y);
            Debug.WriteLine($"Q={Q0} {Q1}");

            Owner.UpdateTransform();
            Owner.Refresh();
        }        
    }

    public class RotatePoint : ControlPoint
    {
        public RotatePoint(VisualComponent owner) : base(owner, 0.5f, -0.2f)
        {
        }

        Point Origin;
        Point StartPoint;
        Matrix InvTransform;
        float Rotation;

        public override void OnMoveStarted(Point position)
        {
            Origin = new Point(Owner.Location.X + Owner.Size.Width / 2, Owner.Location.Y + Owner.Size.Height / 2);
            Point[] p = new Point[1] { GetRealPosition() };
            InvTransform = Owner.Transform.Clone();
            InvTransform.Invert();
            InvTransform.TransformPoints(p);
            StartPoint = p[0];
            Rotation = Owner.Rotation;
        }

        public override void OnMoved(Point newPosition)
        {
            Point[] p = new Point[1] { newPosition };
            InvTransform.TransformPoints(p);
            var MovePoint = p[0];
            var v = new Point(StartPoint.X - Origin.X, StartPoint.Y - Origin.Y);
            var w = new Point(MovePoint.X - Origin.X, MovePoint.Y - Origin.Y);
            var nv = Math.Sqrt(v.X * v.X + v.Y * v.Y);
            var nw = Math.Sqrt(w.X * w.X + w.Y * w.Y);
            if (nv == 0 || nw == 0)            
                return;

            var cos = ((v.X * w.X + v.Y * w.Y) / (nv * nw));
            var sin = ((v.X * w.Y - v.Y * w.X) / (nv * nw));

            var angle = (float)(Math.Atan2(sin, cos) * 180 / Math.PI);
            Owner.Rotation = Rotation + angle;                       
        }

    }
}
