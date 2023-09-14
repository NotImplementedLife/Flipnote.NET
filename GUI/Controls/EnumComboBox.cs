using FlipnoteDotNet.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Controls
{
    internal class EnumComboBox<E> : ComboBox where E:struct, IConvertible
    {
        protected class Item
        {
            public string Name { get; }
            public E EnumInstance { get; }

            public Item(E enumInstance)
            {                
                EnumInstance = enumInstance;
                Name = Enum.GetName(typeof(E), EnumInstance);
            }
        }

        protected List<Item> Values = new List<Item>();
        BindingList<Item> BindingValues;
        public EnumComboBox()
        {
            if (!typeof(E).IsEnum)
                throw new InvalidOperationException("EnumComboBox template argument must be of an enum type");

            foreach (E e in Enum.GetValues(typeof(E)))
                Values.Add(new Item(e));            

            DataSource = null;
            BindingValues = new BindingList<Item>(Values);            
            DataSource = BindingValues;            
            DisplayMember = "Name";
            SelectedIndexChanged += EnumComboBox_SelectedIndexChanged;            
            
        }
        
        private void EnumComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedIndex < 0)
                SelectedIndex = 0;            
        }

        public new E SelectedItem
        {
            get => (base.SelectedItem as Item)?.EnumInstance ?? Values[0].EnumInstance;
            set => base.SelectedItem = Values.Where(_ => Equals(_.EnumInstance, value)).FirstOrDefault() ?? Values[0];
        }
    }
}
