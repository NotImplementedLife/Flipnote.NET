using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace G_TypeConsole
{
    public class TcsEditor : FrameworkElement
    {
        public TcsEditor()
        {
            Lines.Add(new Line());
            Lines.Add(new Line("# this is a comment"));
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(Brushes.White, null, new Rect(0, 0, ActualWidth, ActualHeight));
            for (int i = 0; i < Lines.Count; i++)
            {
                Debug.WriteLine(i);
                drawingContext.DrawText(
                    new FormattedText($"{i + 1}".PadLeft(3, ' '), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, rTypeface, rFontSize, Brushes.Gray),
                    new Point(5, rFontSize * i)
                );
                Lines[i].Render(drawingContext, new Point(50, rFontSize * i));
            }
        }

        private List<Line> Lines = new List<Line>();

        private static FontFamily rFontFamily = new FontFamily("Lucida Console");
        private static Typeface rTypeface = new Typeface(rFontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
        private static int rCharW = 10;
        private static int rFontSize = 16;

        class Line
        {
            public Line(string content="")
            {
                Content = content;
            }
            public string Content = "";

            public void Render(DrawingContext ctx, Point pt)
            {
                if (Content == "") return;
                if(Content[0]=='#')
                {
                    ctx.DrawText(new FormattedText(Content, CultureInfo.CurrentCulture, 
                        FlowDirection.LeftToRight, rTypeface, rFontSize, Brushes.Green), pt);
                }
                else
                {
                    
                }
            }

        }

        List<string> Keywords = new List<string>
        {
            "cls",
            "delay",
            "type",
            "write",
        };
    }
}
