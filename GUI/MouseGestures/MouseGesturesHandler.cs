using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.MouseGestures
{
    internal class MouseGesturesHandler
    {
        public Control Target { get; private set; }
        public MouseGesturesHandler()
        {
            
        }

        public void AttachTarget(Control target)
        {
            if (Target != null) DetachTarget();
            if (target == null) return;
            Target = target;

            Target.MouseDown += Target_MouseDown;
            Target.MouseMove += Target_MouseMove;
            Target.MouseUp += Target_MouseUp;
            Target.MouseEnter += Target_MouseEnter;
            Target.MouseLeave += Target_MouseLeave;
            Target.MouseWheel += Target_MouseWheel;
        }        

        public void DetachTarget()
        {
            if(Target!=null)
            {
                Target.MouseDown -= Target_MouseDown;
                Target.MouseMove -= Target_MouseMove;
                Target.MouseUp -= Target_MouseUp;
                Target.MouseEnter -= Target_MouseEnter;
                Target.MouseLeave -= Target_MouseLeave;
                Target.MouseWheel -= Target_MouseWheel;
            }
            Target = null;
        }


        private void Target_MouseWheel(object sender, MouseEventArgs e)
        {            
            var factor = e.Delta / SystemInformation.MouseWheelScrollDelta;
            if (factor == 0) return;
            Zoom?.Invoke(Target, new ZoomGestureArgs(e.Location, factor));
        }

        private bool IsMouseDown;
        private bool HasMouseMoved;
        private bool IsDragging;
        private Point MouseDownLocation;
        private Point DragCurrentLocation;
        private object DragUserData;

        private void Target_MouseDown(object sender, MouseEventArgs e)
        {
            switch(e.Button)
            {
                case MouseButtons.Left:
                    IsMouseDown = true;
                    HasMouseMoved = false;
                    IsDragging = false;
                    MouseDownLocation = e.Location;
                    DragCurrentLocation = e.Location;
                    break;                
            }            

            
        }

        private void Target_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsMouseDown) return;

            bool prevHasMouseMoved = HasMouseMoved;

            HasMouseMoved = true;

            DragCurrentLocation = e.Location;

            if (!prevHasMouseMoved)
            {
                if (!IsDragging)
                {
                    IsDragging = true;
                    ManageDragStart();
                }
            }
            else if (IsDragging) 
            {
                ManageDrag();
            }
        }

        private void Target_MouseUp(object sender, MouseEventArgs e)
        {
            if (!IsMouseDown) return;
            IsMouseDown = false;

            if (!HasMouseMoved)
                ManageClick();
            else if (IsDragging)
                ManageDrop();          
        }

        private void Target_MouseLeave(object sender, System.EventArgs e)
        {
            IsMouseDown = false;           
            if(IsDragging)
            {
                ManageDrop();
            }
        }

        private void Target_MouseEnter(object sender, System.EventArgs e)
        {
            IsMouseDown = false;
            if(IsDragging)
            {
                ManageDrop();
            }
        }

        private void ManageDragStart()
        {
            DragUserData = null;
            var ev = new DragGestureArgs(MouseDownLocation, DragCurrentLocation);
            DragStart?.Invoke(Target, ev);

            if (ev.IsCanceled)
            {
                IsDragging = false;
                return;
            }
            DragUserData = ev.UserData;
        }

        private void ManageDrag()
        {
            var ev = new DragGestureArgs(MouseDownLocation, DragCurrentLocation);
            ev.UserData = DragUserData;
            Drag?.Invoke(Target, ev);

            if (ev.IsCanceled)
            {
                ManageDrop();
                return;
            }
            DragUserData = ev.UserData;

        }

        private void ManageDrop()
        {            
            IsDragging = false;
            Drop?.Invoke(Target, new DropGestureArgs(MouseDownLocation, DragCurrentLocation, DragUserData));
        }

        private void ManageClick()
        {
            Click?.Invoke(Target, new ClickGestureArgs(MouseDownLocation));
        }

        public event ClickGesture Click;
        public event DragStartGesture DragStart;        
        public event DragGesture Drag;
        public event DropGesture Drop;
        public event ZoomGesture Zoom;
    }
}
