using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FlipnoteDotNet.Utils.Manipulator
{
    using TargetType = Type;
    using ManipulatorType = Type;        

    public class ManipulatorsManager
    {
        Dictionary<TargetType, List<ManipulatorType>> Manipulators;
        public ManipulatorsManager()
        {
            ScanTypes();
        }

        private void ScanTypes()
        {
            Manipulators = AppDomain.CurrentDomain.GetAssemblies()
                      .SelectMany(t => t.GetTypes())
                      .Where(t => t.GetCustomAttribute<ManipulatesAttribute>() != null)
                      .GroupBy(t => t.GetCustomAttribute<ManipulatesAttribute>().TargetType)
                      .ToDictionary(g => g.Key, g => g.ToList());              
        }

        public ManipulatorType GetManipulator(TargetType targetType, Type baseType)
        {
            if (!Manipulators.TryGetValue(targetType, out var list))
                return null;
            if (baseType.IsInterface)
                return list.Where(_ => _.GetInterfaces().Contains(baseType)).FirstOrDefault();
            return list.Where(_ => _.IsSubclassOf(baseType)).FirstOrDefault();            
        }

        public ManipulatorType GetManipulator(object target, Type baseType)
        {
            if (target == null) return null;
            return GetManipulator(target.GetType(), baseType);
        }

        public ManipulatorType GetManipulator<T>(object target)
        {
            if (target == null) return null;
            return GetManipulator(target.GetType(), typeof(T));
        }

    }
}
