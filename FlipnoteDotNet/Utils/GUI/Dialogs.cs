using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Utils.GUI
{
    public static class Dialogs
    {
        public static bool RequestOpenFile(out string filename, string filter = "")
        {
            using var dialog = new OpenFileDialog { Filter = filter };
            return (filename = dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null) != null;
        }

        public static bool RequestSaveFile(out string filename, string filter = "")
        {
            using var dialog = new SaveFileDialog { Filter = filter };
            return (filename = dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null) != null;
        }
    }
}
