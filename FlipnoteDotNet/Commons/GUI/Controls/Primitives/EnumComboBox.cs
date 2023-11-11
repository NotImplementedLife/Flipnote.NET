using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FlipnoteDotNet.Commons.GUI.Controls.Primitives
{
    internal class EnumComboBox<E> : ComboBox where E:struct, IConvertible
    {
        protected class Item
        {
            public string Name { get; }
            public E EnumInstance { get; }
            public int Index { get; }

            public Item(E enumInstance, int index)
            {                
                EnumInstance = enumInstance;
                Name = Enum.GetName(typeof(E), EnumInstance);
                Index = index;
            }
        }

        protected List<Item> Values = new List<Item>();        
        public EnumComboBox()
        {
            if (!typeof(E).IsEnum)
                throw new InvalidOperationException("EnumComboBox template argument must be of an enum type");

            int i = 0;
            foreach (E e in Enum.GetValues(typeof(E)))
                Values.Add(new Item(e, i++));

            Items.AddRange(Values.ToArray());
            
            DisplayMember = "Name";
            SelectedIndex = 0;
            SelectedIndexChanged += EnumComboBox_SelectedIndexChanged;            
            
        }
        
        private void EnumComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedIndex < 0)
                SelectedIndex = 0;            
        }

        public E SelectedEnumItem
        {
            get
            {
                return Values[SelectedIndex < 0 ? 0 : SelectedIndex].EnumInstance;
            }
            set => SelectedIndex = (Values.Where(_ => Equals(_.EnumInstance, value)).FirstOrDefault() ?? Values[0]).Index;
        }
    }
}
