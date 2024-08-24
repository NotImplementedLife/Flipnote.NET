using System.ComponentModel;

namespace FlipnoteDotNet.App.Controls
{
    public class BoundedListView<T> : ListView
    {
        public BoundedListView() 
        {
            TargetList = new List<T>();
        }

        private IList<T> fTargetList;
        private BindingList<T> fBindingList;

        public IList<T> TargetList
        {
            get => fBindingList;
            set
            {
                if (fTargetList == value) return;
                fTargetList = value;
                if(fBindingList!=null)
                {
                    fBindingList.ListChanged -= FBindingList_ListChanged;                   
                    fBindingList = null;                    
                }
                if(!(fTargetList is BindingList<T> alreadyBindingList))
                {
                    fBindingList = new BindingList<T>(fTargetList);
                    fBindingList.ListChanged += FBindingList_ListChanged;
                }
                else
                {
                    fBindingList = alreadyBindingList;
                    fBindingList.ListChanged += FBindingList_ListChanged;
                }
                
                LoadItems();
            }
        }    

        private void FBindingList_ListChanged(object sender, ListChangedEventArgs e)
        {            
            switch(e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    {
                        OnItemAdded(e.NewIndex, fBindingList[e.NewIndex]);
                        break;
                    }
                case ListChangedType.ItemDeleted:
                    {
                        OnItemRemoved(e.NewIndex);
                        break;
                    }
                case ListChangedType.ItemMoved:
                    {
                        throw new NotImplementedException("ItemMoved?");                        
                    }
                case ListChangedType.ItemChanged:
                    {
                        break;
                    }                
                case ListChangedType.Reset:
                    {
                        break;
                    }
            }
            
        }

        protected virtual void OnItemAdded(int index, T item)
        {
            Items.Insert(index, CreateListViewItem(item));
        }

        protected virtual void OnItemRemoved(int index)
        {            
            Items.RemoveAt(index);
        }

        private void LoadItems()
        {
            SuspendLayout();
            Items.Clear();
            for(int i=0;i<fBindingList.Count;i++)
            {
                Items.Add(CreateListViewItem(fBindingList[i]));
            }
            ResumeLayout(true);
        }

        protected virtual ListViewItem CreateListViewItem(T value)
        {
            return new ListViewItem(value?.ToString() ?? "null") { Tag = value };
        }
    }
}
