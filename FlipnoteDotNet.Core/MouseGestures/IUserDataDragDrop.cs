using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Core.MouseGestures
{
    public interface IUserDataDragDrop
    {
        void OnDrag(object sender, DragGestureArgs e);
        void OnDrop(object sender, DropGestureArgs e);
    }
}
