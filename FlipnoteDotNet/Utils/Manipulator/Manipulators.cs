using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Utils.Manipulator
{
    public static class Manipulators
    {
        private static ManipulatorsManager ManipulatorsManager;

        public static void Initialize()
        {
            ManipulatorsManager = new ManipulatorsManager();
        }

        public static Type GetManipulatorType<T>(object target) => ManipulatorsManager.GetManipulator<T>(target);

        public static T CreateManipulator<T>(object target) where T : class
        {
            var type = GetManipulatorType<T>(target);
            if (type == null) return null;
            return Activator.CreateInstance(type) as T;
        }
    }
}
