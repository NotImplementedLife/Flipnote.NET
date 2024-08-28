using FlipnoteDotNet.Canvas.Components;
using FlipnoteDotNet.Canvas.Selections;

namespace FlipnoteDotNet.Canvas
{
    internal class SelectionManager
    {
        internal Dictionary<CanvasComponent, SelectionState> Selections = new Dictionary<CanvasComponent, SelectionState>();       

        private void Select(CanvasComponent item)
        {
            var state = item.CreateSelectionState();
            state.CaptureBaseState();
            Selections.Add(item, state);
        }

        private void Deselect(CanvasComponent item)
        {
            Selections[item].Detach();
            Selections.Remove(item);
        }

        public void ToggleSelection(CanvasComponent item)
        {
            if (Selections.ContainsKey(item))
                Deselect(item);
            else
                Select(item);
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ClearSelection()
        {
            Selections.Clear();
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public void SelectSingle(CanvasComponent item)
        {
            Selections.Clear();
            Select(item);
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler SelectionChanged;        

        public CanvasComponent[] GetSelectedComponents() => Selections.Keys.ToArray();
        public bool IsSelected(CanvasComponent component) => Selections.ContainsKey(component);

        public void OnPaint(Graphics g)
        {
            foreach (var state in Selections.Values) 
            {
                state.OnPaint(g);
            }
        }

        public bool ControlPointHitTest(PointF point, out SelectionState selectionState)
        {
            foreach (var (component, state) in Selections) 
            {
                int index = state.HitTest(point);
                if (index >= 0)
                {
                    state.SetActivePoint(index);
                    selectionState = state;
                    return true;
                }
            }
            selectionState = null;
            return false;
        }

        public bool ControlPointHitTest(CanvasComponent component, PointF point)
        {
            if (!Selections.TryGetValue(component, out var state)) return false;
            int index = state.HitTest(point);
            if (index == -1) return false;
            state.SetActivePoint(index);
            return true;
        }

    }
}
