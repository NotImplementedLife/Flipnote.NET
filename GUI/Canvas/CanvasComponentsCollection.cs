using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.GUI.Canvas
{
    public partial class CanvasSpaceControl
    {
        public class CanvasComponentsCollection : IList<ICanvasComponent>
        {

            public CanvasSpaceControl Canvas { get; }

            public CanvasComponentsCollection(CanvasSpaceControl canvas)
            {
                Canvas = canvas;
            }

            private List<ICanvasComponent> CanvasComponents { get; } = new List<ICanvasComponent>();            
            private HashSet<ICanvasComponent> _SelectedComponents { get; } = new HashSet<ICanvasComponent>();

            #region Selection

            public void SetSelected(ICanvasComponent component, bool isSelected)
            {
                if (!CanvasComponents.Contains(component)) return;
                if (isSelected)
                    _SelectedComponents.Add(component);
                else
                    _SelectedComponents.Remove(component);

                Canvas.SelectionChanged?.Invoke(Canvas, new EventArgs());
            }

            public void ToggleSelected(ICanvasComponent component)
            {
                if (!CanvasComponents.Contains(component)) return;
                if (_SelectedComponents.Contains(component))
                    _SelectedComponents.Remove(component);
                else
                    _SelectedComponents.Add(component);
                Canvas.SelectionChanged?.Invoke(Canvas, new EventArgs());
            }

            public bool IsSelected(ICanvasComponent component)
            {
                return _SelectedComponents.Contains(component);
            }

            public void SelectSingle(ICanvasComponent component)
            {
                if (component == null)
                {
                    if (_SelectedComponents.Count > 0)
                    {
                        _SelectedComponents.Clear();
                        Canvas.SelectionChanged?.Invoke(Canvas, new EventArgs());
                    }
                    return;
                }

                if (!CanvasComponents.Contains(component)) return;
                _SelectedComponents.Clear();                
                _SelectedComponents.Add(component);
                Canvas.SelectionChanged?.Invoke(Canvas, new EventArgs());
            }

            public bool IsSelectionEmpty => _SelectedComponents.Count == 0;

            public IEnumerable<ICanvasComponent> SelectedComponents => _SelectedComponents.AsEnumerable();
            public int SelectedComponentsCount => _SelectedComponents.Count;

            #endregion


            #region IList<> implementation

            public ICanvasComponent this[int index] { get => CanvasComponents[index]; set => throw new InvalidOperationException(); }
            public int Count => CanvasComponents.Count;
            public bool IsReadOnly => false;

            public void Add(ICanvasComponent item)
            {
                if (!CanvasComponents.Contains(item))
                    CanvasComponents.Add(item);
            }

            public void Clear()
            {
                CanvasComponents.Clear();
                _SelectedComponents.Clear();
            }

            public bool Contains(ICanvasComponent item) => CanvasComponents.Contains(item);           

            public void CopyTo(ICanvasComponent[] array, int arrayIndex)
            {
                foreach (var item in CanvasComponents)
                    array[arrayIndex++] = item;                
            }

            public int IndexOf(ICanvasComponent item) => CanvasComponents.IndexOf(item);

            public void Insert(int index, ICanvasComponent item)
            {
                throw new InvalidOperationException();                
            }

            public bool Remove(ICanvasComponent item)
            {
                _SelectedComponents.Remove(item);
                return CanvasComponents.Remove(item);
            }

            public void RemoveAt(int index)
            {
                _SelectedComponents.Remove(CanvasComponents[index]);
                CanvasComponents.RemoveAt(index);
            }

            public IEnumerator<ICanvasComponent> GetEnumerator() => CanvasComponents.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => CanvasComponents.GetEnumerator();
            #endregion
        }
    }
}
