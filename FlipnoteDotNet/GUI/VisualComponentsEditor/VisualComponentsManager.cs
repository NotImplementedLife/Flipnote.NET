using FlipnoteDotNet.Commons.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace FlipnoteDotNet.GUI.VisualComponentsEditor
{
    public class VisualComponentsManager
    {
        private readonly List<VisualComponent> Components = new List<VisualComponent>();
        private readonly HashSet<VisualComponent> Selection = new HashSet<VisualComponent>();
        public BitmapProcessor BitmapProcessor { get; } = new BitmapProcessor();       

        public VisualComponentsManager()
        {
            BitmapProcessor.RunAsync();
        }

        public void Clear()
        {
            Components.Clear();
            ClearSelection();
        }

        public void AddComponent(VisualComponent component)
        {
            Components.Add(component);
        }

        public void RemoveComponent(VisualComponent component)
        {
            if (Components.Remove(component))
                RemoveSelection(component); 
        }

        public IEnumerable<VisualComponent> GetComponents() => Components;

        public void ClearSelection() 
        {
            if (Selection.Count > 0)
            {
                Selection.Clear();
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void AddSelection(VisualComponent component)
        {
            if(Selection.Add(component))                            
                SelectionChanged?.Invoke(this, EventArgs.Empty);            
        }

        public void RemoveSelection(VisualComponent component)
        {
            if(Selection.Remove(component))            
                SelectionChanged?.Invoke(this, EventArgs.Empty);            
        }

        public event EventHandler SelectionChanged;

        public void TriggerSelect(Point position, bool overwrite = true)
        {
            VisualComponent target = null;
            for (int i = 0; i < Components.Count; i++)
                if (Components[i].HitTest(position))
                //if (new Rectangle(Components[i].Location, Components[i].Size).Contains(position))
                    target = Components[i];

            if(overwrite)
            {
                ClearSelection();
                if (target != null) AddSelection(target);
                return;
            }
            if (target == null) return;
            if (Selection.Contains(target))
                RemoveSelection(target);
            else
                AddSelection(target);
        }

        public IEnumerable<VisualComponent> GetSelectedComponents() => Selection;
        public int SelectionCount => Selection.Count;
    }
}
