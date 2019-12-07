using System;
using System.Collections.ObjectModel;
using System.Linq;
using ProjetXamarinWear.Models;

namespace ProjetXamarinWear.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ObservableCollection<Item> UserItems { get; set; }
        public Boolean fav { get; set; }
        public ItemDetailViewModel(Item item, ItemsViewModel allMessage)
        {
            Title = item?.student_message;
            Item = item;
            fav = false;
            getMessageOfUser(item.student_id, allMessage);
        }

        private void getMessageOfUser(long id, ItemsViewModel allMessage) {
            this.UserItems = new ObservableCollection<Item>();
            foreach (Item m in allMessage.Items.ToList())
            {
                if (m.student_id == id)
                    this.UserItems.Add(m);
            }
        }
    }
}
