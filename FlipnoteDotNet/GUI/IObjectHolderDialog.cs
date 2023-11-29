using System.Windows.Forms;

namespace FlipnoteDotNet.GUI
{
    public interface IObjectHolderDialog
    {
        object ObjectValue { get; set; }
        DialogResult ShowDialog();
    }
}
