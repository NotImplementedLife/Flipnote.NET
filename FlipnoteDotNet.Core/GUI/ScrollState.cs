namespace FlipnoteDotNet.Core.GUI
{
    internal class ScrollState
    {
        internal bool Enabled = true;
        internal int ContentLength;
        internal int Position = 0;
        internal int ThumbLength;
        internal int ThumbThickness = 10;
        internal Rectangle TrackRectangle;
        internal Rectangle ThumbRectangle;
        internal float Scale;
        internal bool Hidden = false;

        internal bool MsDown = false;
        internal int DownPos;
        internal int ThumbPos;

        internal event EventHandler<int> ScrollChanged;

        internal void UpdateH(Control control)
        {
            if (!Enabled) return;
            if (ContentLength <= control.Width) 
            {
                Hidden = true;
                Position = 0;
                TriggerScrollChanged();
            }
            else
            {
                int trackWidth = control.Width;
                int trackHeight = ThumbThickness;
                int trackX = 0;
                int trackY = control.Height - trackHeight;
                TrackRectangle = new Rectangle(trackX, trackY, trackWidth, trackHeight);
                ThumbLength = Math.Max(
                    trackWidth * control.Width / (ContentLength + control.Width),
                    ThumbThickness * 3 / 2
                    );
                Scale = 1f * (ContentLength - control.Width) / (trackWidth - ThumbLength);
                ThumbRectangle = new Rectangle((int)(Position / Scale), trackY,
                    ThumbLength, trackHeight);
                Hidden = false;
            }
        }


        internal void UpdateV(Control control)
        {
            if (!Enabled) return;
            if (ContentLength <= control.Height) 
            {
                Hidden = true;
                Position = 0;
                TriggerScrollChanged();
            }
            else
            {
                int trackWidth = ThumbThickness;
                int trackHeight = control.Height;
                int trackX = control.Width - trackWidth;
                int trackY = 0;
                TrackRectangle = new Rectangle(trackX, trackY, trackWidth, trackHeight);
                ThumbLength = Math.Max(
                    trackHeight * control.Height / (ContentLength + control.Height),
                    ThumbThickness * 3 / 2
                    );
                Scale = 1f * (ContentLength - control.Height) / (trackHeight - ThumbLength);                
                ThumbRectangle = new Rectangle(trackX, (int)(Position / Scale), trackWidth, ThumbLength);
                Hidden = false;
            }
        }

        internal void TriggerScrollChanged() => ScrollChanged?.Invoke(this, Position);

        internal void Draw(Graphics g, Brush trackBrush, Brush thumbBrush)
        {
            if (Enabled && !Hidden) 
            {
                g.FillRectangle(trackBrush, TrackRectangle);
                g.FillRectangle(thumbBrush, ThumbRectangle);
            }
        }
    }
}
