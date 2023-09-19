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

    }
}
