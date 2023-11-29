using System.Collections.Generic;
using System;
using System.Windows.Forms;
using System.Linq;

namespace FlipnoteDotNet.Commons.GUI.Controls.Primitives
{
    internal class StaticComboBox<T> : ComboBox
    {
        protected class Item
        {            
            public T Instance { get; }
            public int Index { get; }

            public Item(T instance, int index)
            {
                Instance = instance;                
                Index = index;
            }
        }

        protected List<Item> Values = new List<Item>();
        public StaticComboBox(T[] items) : base()
        {
            for (int i = 0; i < items.Length; i++)
                Values.Add(new Item(items[i], i));

            Items.AddRange(Values.ToArray());            
            SelectedIndex = 0;
            SelectedIndexChanged += EnumComboBox_SelectedIndexChanged;

        }

        private void EnumComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedIndex < 0)
                SelectedIndex = 0;
        }

        public T SelectedValueItem
        {
            get => Values[SelectedIndex < 0 ? 0 : SelectedIndex].Instance;
            set => SelectedIndex = (Values.Where(_ => ItemEquals(_.Instance, value)).FirstOrDefault() ?? Values[0]).Index;
        }

        protected virtual bool ItemEquals(object a, object b) => Equals(a, b);        
    }
}
