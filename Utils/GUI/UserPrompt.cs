using System.Windows.Forms;

namespace FlipnoteDotNet.Utils.GUI
{
    internal class UserPrompt
    {
        public static bool Accepts(string message, string caption = "Prompt")
        {
            return MessageBox.Show(message, caption, MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

    }
}
