using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Menu
{
    public class MenuItem<F> : ToolStripMenuItem where F : Form
    {
        public F Form { get; }

        public MenuItem(F form)
        {
            Form = form;
        }
    }
}
