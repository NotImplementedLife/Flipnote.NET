using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace G_TypeConsole
{
    public partial class GenSettingsWindow : Form
    {
        public GenSettingsWindow()
        {
            InitializeComponent();
            Editor.AppendText("cls\nwrite \"Hello world!\"\ndelay 1000\n");
        }

        // https://codingvision.net/c-simple-syntax-highlighting
        private void Editor_TextChanged(object sender, EventArgs e)
        {
            MatchCollection keywordMatches = Regex.Matches(Editor.Text, @"\b(cls|delay|fontsize|type|write|writeln)\b");
            MatchCollection commentMatches = Regex.Matches(Editor.Text, @"(^#.*$)");
            MatchCollection stringMatches = Regex.Matches(Editor.Text, "\".*?\"");
            MatchCollection numberMatches = Regex.Matches(Editor.Text, "(?<![A-Za-z0-9.])[0-9]+(?![A-Za-z0-9.])");

            int originalIndex = Editor.SelectionStart;
            int originalLength = Editor.SelectionLength;
            Color originalColor = Color.Black;

            PreviewButton.Focus();

            Editor.SelectionStart = 0;
            Editor.SelectionLength = Editor.Text.Length;
            Editor.SelectionColor = originalColor;

            foreach (Match m in keywordMatches)
            {
                Editor.SelectionStart = m.Index;
                Editor.SelectionLength = m.Length;
                Editor.SelectionColor = Color.Blue;
            }

            foreach (Match m in commentMatches)
            {
                Editor.SelectionStart = m.Index;
                Editor.SelectionLength = m.Length;
                Editor.SelectionColor = Color.Green;
            }

            foreach (Match m in numberMatches)
            {
                Editor.SelectionStart = m.Index;
                Editor.SelectionLength = m.Length;
                Editor.SelectionColor = Color.DarkMagenta;
            }

            foreach (Match m in stringMatches)
            {
                Editor.SelectionStart = m.Index;
                Editor.SelectionLength = m.Length;
                Editor.SelectionColor = Color.Brown;
            }

            Editor.SelectionStart = originalIndex;
            Editor.SelectionLength = originalLength;
            Editor.SelectionColor = originalColor;

            Editor.Focus();
        }

        // https://stackoverflow.com/questions/4683663/how-to-remove-annoying-beep-with-richtextbox
        private void Editor_KeyDown(object sender, KeyEventArgs e)
        {
            if (Editor.GetLineFromCharIndex(Editor.SelectionStart) == 0 && e.KeyData == Keys.Up ||
                Editor.GetLineFromCharIndex(Editor.SelectionStart) == Editor.GetLineFromCharIndex(Editor.TextLength) && e.KeyData == Keys.Down ||
                Editor.SelectionStart == Editor.TextLength && e.KeyData == Keys.Right ||
                Editor.SelectionStart == 0 && e.KeyData == Keys.Left)
                e.Handled = true;
        }
    }
}
