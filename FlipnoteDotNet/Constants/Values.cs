using System.ComponentModel;
using System.Windows.Forms;

namespace FlipnoteDotNet
{
    public static partial class Constants
    {
        public static Control SnychronizingObject;

        public static void Init()
        {
            SnychronizingObject = new Control();            
            Reflection.Init();
        }


        public static bool IsDesignerMode => (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
    }
}
