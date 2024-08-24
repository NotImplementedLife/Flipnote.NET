using FlipnoteDotNet.Core.MouseGestures;
using FlipnoteDotNet.Core.Utils;
using System.Diagnostics;

namespace FlipnoteDotNet.Utils.GUI
{
    public class ColumnsSplitter
    {        
        private readonly DisplayLength[] ColumnWidths;
        public readonly int ColumnsCount;
        private Control Container;
        private readonly int[] RealWidths;
        private readonly int[] SplittersX;
        int Padding = 5;

        MouseGesturesHnd fMouseGesturesHnd;

        private class MouseGesturesHnd : MouseGesturesHandler
        {
            ColumnsSplitter ColumnsSplitter;
            int SplitterId;
            int X0;

            public MouseGesturesHnd(ColumnsSplitter columnsSplitter)
            {
                ColumnsSplitter = columnsSplitter;                
            }

            protected override void OnDragStart(DragGestureArgs e)
            {
                if (!ColumnsSplitter.HoversSplitter)
                {
                    e.Cancel();
                    return;
                }
                SplitterId = ColumnsSplitter.HoveredSplitterId;
                X0 = ColumnsSplitter.SplittersX[SplitterId];
            }

            protected override void OnDrag(DragGestureArgs e)
            {
                ColumnsSplitter.SetSplitterPosition(SplitterId, X0 + e.DeltaLocation.X);
            }            
        }

        public ColumnsSplitter(int columnsCount)
        {
            ColumnsCount = columnsCount;            
            ColumnWidths = Arrays.Initialize(ColumnsCount, DisplayLength.Proportional(1));
            RealWidths = new int[ColumnsCount];
            SplittersX = new int[ColumnsCount];
            fMouseGesturesHnd = new MouseGesturesHnd(this);
        }

        public ColumnsSplitter(DisplayLength[] colWidths)
        {
            ColumnsCount = colWidths.Length;
            ColumnWidths = colWidths.ToArray();
            RealWidths = new int[ColumnsCount];
            SplittersX = new int[ColumnsCount];
            fMouseGesturesHnd = new MouseGesturesHnd(this);
        }
        
        public void Attach(Control container)
        {
            Detach();
            Container = container;
            Container.Resize += Container_Resize;
            Container.MouseMove += Container_MouseMove;
            Container.Paint += Container_Paint;
            fMouseGesturesHnd.AttachTarget(Container);
            AdjustChildren();
        }

        public void Detach()
        {
            if (Container == null) return;
            Container.Resize -= Container_Resize;
            Container.MouseMove -= Container_MouseMove;
            Container.Paint -= Container_Paint;
            fMouseGesturesHnd.DetachTarget();
        }

        private void Container_Paint(object sender, PaintEventArgs e)
        {
            int y0 = Container.Height / 4;
            int y1 = Container.Height * 3 / 4;
            for(int i=0;i<SplittersX.Length-1;i++)
            {
                int x = SplittersX[i];
                e.Graphics.DrawLine(Pens.Black, x, y0, x, y1);
            }
        }
        private bool HoversSplitter = false;
        private int HoveredSplitterId = -1;

        private bool PreviouslyHoveredSplitter = false;

        private void Container_MouseMove(object sender, MouseEventArgs e)
        {             
            HoversSplitter = false;            
            for (int i=0;i<ColumnsCount-1;i++)
            {
                if (Math.Abs(e.X - SplittersX[i]) <= 5) 
                {
                    HoversSplitter = true;
                    HoveredSplitterId = i;
                    break;
                }
            }
            if(PreviouslyHoveredSplitter)
            {
                Cursor.Current = HoversSplitter ? Cursors.SizeWE : Cursors.Default;
            }                        
            PreviouslyHoveredSplitter = HoversSplitter;
        }        

        private void Container_Resize(object sender, EventArgs e)
        {
            AdjustChildren();
            Container.Invalidate();
        }

        private void AdjustChildren()
        {
            UpdateRealWidths(Container.Width);
            int x = Padding;
            Container.SuspendLayout();
            for (int i = 0; i < ColumnsCount; i++)
            {
                Container.Controls[i].Left = x;
                Container.Controls[i].Width = RealWidths[i];
                Container.Controls[i].Height = Container.Height;
                x += RealWidths[i] + Padding;
            }
            Container.ResumeLayout(true);            
        }

        private void SetSplitterPosition(int index, int x)
        {
            if (ColumnWidths[index].IsProportional && ColumnWidths[index + 1].IsProportional)
            {
                var dx = x - SplittersX[index];
                float dp = dx * ProportionalWhole / Container.Width;
                if (ColumnWidths[index].Value + dp >= ProportionalWhole || ColumnWidths[index + 1].Value < dp + 0.1f
                    || ColumnWidths[index].Value < -dp + 0.1f) 
                    return;                

                ColumnWidths[index] += dp;
                ColumnWidths[index + 1] -= dp;
            }
            else
            {
                throw new NotImplementedException("TO DO: Splitter px");
            }
            AdjustChildren();
            Container.Invalidate();
        }

        private float ProportionalWhole;
        private float LeftProportionalSpace;

        private void UpdateRealWidths(int totalWidth)
        {
            LeftProportionalSpace = totalWidth - (ColumnsCount + 1) * Padding;
            ProportionalWhole = 0;

            for (int i = 0; i < ColumnWidths.Length; i++)
            {
                if (ColumnWidths[i].IsPixels)
                {
                    LeftProportionalSpace -= ColumnWidths[i].Value;
                    RealWidths[i] = (int)ColumnWidths[i].Value;
                }
                else if (ColumnWidths[i].IsProportional)
                    ProportionalWhole += ColumnWidths[i].Value;
            }

            int x = Padding;
            for (int i = 0; i < ColumnWidths.Length; i++)
            {
                if (ColumnWidths[i].IsProportional)
                {
                    RealWidths[i] = (int)(ColumnWidths[i].Value * LeftProportionalSpace / ProportionalWhole);
                }
                x += RealWidths[i]+Padding;
                SplittersX[i] = x - Padding / 2 - (Padding % 2);
            }
        }
    }
}
