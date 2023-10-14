using FlipnoteDotNet.Data;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Forms
{
    public interface ILayerCreatorForm
    {
        ILayer Layer { get; }
        DialogResult ShowDialog();
    }
}
