using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Controls
{
    public class StaticListBox<T> : ListBox
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
        public StaticListBox(T[] items) : base()
        {
            for (int i = 0; i < items.Length; i++)
                Values.Add(new Item(items[i], i));

            Items.AddRange(Values.ToArray());
            SelectedIndex = 0;
            SelectedIndexChanged += EnumListBox_SelectedIndexChanged;
        }

        private void EnumListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedIndex < 0)
                SelectedIndex = 0;
        }

        public T SelectedValueItem
        {
            get => Values[SelectedIndex < 0 ? 0 : SelectedIndex].Instance;
            set => SelectedIndex = (Values.Where(_ => Equals(_.Instance, value)).FirstOrDefault() ?? Values[0]).Index;
        }
    }
}
