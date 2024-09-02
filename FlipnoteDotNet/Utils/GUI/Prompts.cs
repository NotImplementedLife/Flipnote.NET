using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Utils.GUI
{
    internal static class Prompts
    {
        public static PromptResult SaveBeforeLoadNew()
        {
            var result = MessageBox.Show(String_UnsavedChanged, String_Warning, MessageBoxButtons.YesNoCancel);
            return result switch
            {
                DialogResult.Yes => PromptResult.Save,
                DialogResult.No => PromptResult.Discard,
                _ => PromptResult.Ignore
            };
        }


        private static readonly string String_Warning = "Warning";
        private static readonly string String_UnsavedChanged = "There are unsaved changes. Do you wish to save now?";

    }
}
