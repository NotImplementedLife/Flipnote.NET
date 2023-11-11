using FlipnoteDotNet.GUI.PseudoControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Forms
{
    public class SplitAreaForm : Form
    {
        private readonly List<Area> pAreas = new List<Area>();
        private Brush pBackgroundBrush = GuiConstants.WindowBackgroundBrush;
        private readonly List<PseudoControl> PseudoControls = new List<PseudoControl>();

        public Brush BackgroundBrush
        {
            get => pBackgroundBrush;
            set
            {
                pBackgroundBrush = value;
                Invalidate();
            }       
        }

        public SplitAreaForm()
        {
            DoubleBuffered = true;
            CreateArea();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            pAreas[0].UpdateBounds(ClientRectangle);            
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var msPos = PointToClient(Cursor.Position);

            e.Graphics.FillRectangle(pBackgroundBrush, e.ClipRectangle);

            foreach(var a in pAreas)
            {
                e.Graphics.DrawRectangle(Pens.Red, a.Bounds);
            }

            foreach(var pc in PseudoControls)
            {
                pc.Paint(e.Graphics, pc.Bounds, pc.Bounds.Contains(msPos));
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            pAreas[0].UpdateBounds(ClientRectangle);
        }

        protected override void OnClick(EventArgs e)
        {
            var pos = PointToClient(Cursor.Position);            

            for(int i=0;i<PseudoControls.Count;i++)
            {                
                if (PseudoControls[i].Bounds.Contains(pos))
                {                    
                    PseudoControls[i].PerformClick();
                    break;
                }
            }

            base.OnClick(e);
        }

        protected readonly int WholeAreaId = 0;

        private Area CreateArea()
        {
            var a = new Area(this, pAreas.Count, Rectangle.Empty);
            pAreas.Add(a);
            return a;
        }


        protected int GetAreaId(params int[] areaSelector)
        {
            var a = pAreas[0];
            for (int i = 0; i < areaSelector.Length; i++)
            {
                a = a.Divisions[areaSelector[i]].Area;
            }
            return a.Id;
        }

        private MarginBind ParseMarginBind(int? value)
        {
            if (value == null) return MarginBind.Disabled;
            return MarginBind.Margin(value.Value);
        }

        public void AddControl(int areaId, Control control, int? marginLeft = null, int? marginTop = null, int? marginRight = null, int? marginBottom = null)
        {
            var area = pAreas[areaId];
            var cw = new ControlWrap(control)
            {
                MarginTop = ParseMarginBind(marginTop),
                MarginLeft = ParseMarginBind(marginLeft),
                MarginBottom = ParseMarginBind(marginBottom),
                MarginRight = ParseMarginBind(marginRight),
            };
            Controls.Add(control);
            area.Controls.Add(cw);
            cw.UpdateLayout(area.Bounds);
        }

        public void AddControl(int areaId, PseudoControl control, int? marginLeft = null, int? marginTop = null, int? marginRight = null, int? marginBottom = null)
        {
            var area = pAreas[areaId];
            var cw = new ControlWrap(control)
            {
                MarginTop = ParseMarginBind(marginTop),
                MarginLeft = ParseMarginBind(marginLeft),
                MarginBottom = ParseMarginBind(marginBottom),
                MarginRight = ParseMarginBind(marginRight),
            };

            control.RedrawRequired += (o, e) => Invalidate();

            PseudoControls.Add(control);
            area.Controls.Add(cw);
            cw.UpdateLayout(area.Bounds);
        }


        #region SplitArea
        private static (int, bool) StringToWeight(string w)
        {
            if (w.EndsWith("*"))
                return (int.Parse(w.Substring(0, w.Length - 1)), false);
            if (w.EndsWith("px"))
                w = w.Substring(0, w.Length - 2);
            return (int.Parse(w), true);
        }

        private int[] PSplitArea(int areaId, Orientation orientation, params string[] weights)
        {
            var area = pAreas[areaId];
            var ids = new int[weights.Length];
            area.Orientation = orientation;
            for (int i = 0; i < weights.Length; i++)
            {
                var (w, au) = StringToWeight(weights[i]);
                var divArea = CreateArea();
                area.Divisions.Add((w, au, divArea));
                ids[i] = divArea.Id;
            }
            return ids;
        }

        private int[] PSplitAreaH(int areaId, params string[] weights) => PSplitArea(areaId, Orientation.Horizontal, weights);
        private int[] PSplitAreaV(int areaId, params string[] weights) => PSplitArea(areaId, Orientation.Vertical, weights);

        protected int[] SplitAreaH(int areaId, params string[] weights) => PSplitArea(areaId, Orientation.Horizontal, weights);
        protected int[] SplitAreaV(int areaId, params string[] weights) => PSplitArea(areaId, Orientation.Vertical, weights);

        protected (int, int) SplitAreaV(int areaId, string w1, string w2)
        {
            var s = PSplitAreaV(areaId, w1, w2);
            return (s[0], s[1]);
        }

        protected (int, int) SplitAreaH(int areaId, string w1, string w2)
        {
            var s = PSplitAreaH(areaId, w1, w2);
            return (s[0], s[1]);
        }

        protected (int, int, int) SplitAreaV(int areaId, string w1, string w2, string w3)
        {
            var s = PSplitAreaV(areaId, w1, w2, w3);
            return (s[0], s[1], s[2]);
        }

        protected (int, int, int) SplitAreaH(int areaId, string w1, string w2, string w3)
        {
            var s = PSplitAreaH(areaId, w1, w2, w3);
            return (s[0], s[1], s[2]);
        }
        #endregion SplitArea

        struct MarginBind
        {
            public bool IsEnabled { get; }
            public int Value { get; }

            private MarginBind(bool isEnabled, int value = 0)
            {
                IsEnabled = isEnabled;
                Value = value;
            }

            public static readonly MarginBind Disabled = new MarginBind(false);
            public static MarginBind Margin(int x) => new MarginBind(true, x);
        }

        private class ControlWrap
        {
            public readonly PseudoControl PseudoControl;
            public readonly Control Control;

            public MarginBind MarginTop = MarginBind.Disabled;
            public MarginBind MarginLeft = MarginBind.Disabled;
            public MarginBind MarginBottom = MarginBind.Disabled;
            public MarginBind MarginRight = MarginBind.Disabled;

            public ControlWrap(Control control)
            {
                Control = control;
                Control.Dock = DockStyle.None;
                PseudoControl = null;
            }

            public ControlWrap(PseudoControl control)
            {
                PseudoControl = control;
                Control = null;
            }

            public void UpdateLayout(Rectangle parentBounds)
            {
                if (Control != null)
                {
                    if (MarginTop.IsEnabled) Control.Top = parentBounds.Top + MarginTop.Value;
                    if (MarginLeft.IsEnabled) Control.Left = parentBounds.Left + MarginLeft.Value;
                    if (MarginBottom.IsEnabled) Control.Height = Math.Max(parentBounds.Bottom - MarginBottom.Value - Control.Top, 0);
                    if (MarginRight.IsEnabled) Control.Width = Math.Max(parentBounds.Right - MarginRight.Value - Control.Left, 0);
                }
                else
                {
                    var top = PseudoControl.Bounds.Top;
                    var left = PseudoControl.Bounds.Left;
                    var width = PseudoControl.Bounds.Width;
                    var height = PseudoControl.Bounds.Height;                    

                    if (MarginTop.IsEnabled) top = parentBounds.Top + MarginTop.Value;
                    if (MarginLeft.IsEnabled) left = parentBounds.Left + MarginLeft.Value;
                    if (MarginBottom.IsEnabled) height = Math.Max(parentBounds.Bottom - MarginBottom.Value - PseudoControl.Bounds.Top, 0);
                    if (MarginRight.IsEnabled) width = Math.Max(parentBounds.Right - MarginRight.Value - PseudoControl.Bounds.Left, 0);

                    PseudoControl.Bounds = new Rectangle(left, top, width, height);
                }
            }
        }


        private class Area
        {
            private readonly Control Owner;
            public readonly int Id;
            public Rectangle Bounds;
            public List<(int Weight, bool IsAbsoluteUnit, Area Area)> Divisions = new List<(int, bool, Area)>();
            public List<ControlWrap> Controls = new List<ControlWrap>();
            public Orientation Orientation = Orientation.Horizontal;
            public bool IsVertical => Orientation == Orientation.Vertical;
            public bool IsHorizontal => Orientation == Orientation.Horizontal;

            public Area(Control owner, int id, Rectangle bounds)
            {
                Owner = owner;
                Id = id;
                Bounds = bounds;          
            }

            public void UpdateBounds(Rectangle bounds)
            {
                Bounds = bounds;
                Owner.Invalidate(Bounds);

                for (int i = 0; i < Controls.Count; i++)
                    Controls[i].UpdateLayout(bounds);

                int weightSum = 0, relCompsCount = 0;
                int relSpace = IsHorizontal ? bounds.Width : bounds.Height;
                for (int i = 0; i < Divisions.Count; i++)
                {
                    var (weight, isAbsoluteUnit, _) = Divisions[i];
                    if (!isAbsoluteUnit)
                    {
                        weightSum += weight;
                        relCompsCount++;
                    }
                    else
                        relSpace -= weight;
                }
                if (relSpace < 0) relSpace = 0;


                int li = 0;
                for (int i = 0; i < Divisions.Count; i++) 
                {
                    var (weight, isAbsUnit, area) = Divisions[i];
                    int al;

                    if(isAbsUnit)
                    {
                        al = weight;                        
                    }
                    else
                    {
                        if (weightSum == 0)
                            al = relSpace / relCompsCount;
                        else
                            al = weight * relSpace / weightSum;
                    }

                    if (IsHorizontal)
                        area.UpdateBounds(new Rectangle(bounds.X + li, bounds.Y, al, bounds.Height));
                    else
                        area.UpdateBounds(new Rectangle(bounds.X, bounds.Y + li, bounds.Width, al));
                    li += al;
                }
            }
        }
    }
}
