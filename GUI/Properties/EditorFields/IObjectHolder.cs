using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Properties.EditorFields
{
    public interface IObjectHolderDialog
    {
        object ObjectValue { get; set; }
        DialogResult ShowDialog();
    }
}
