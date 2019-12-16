using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ProjetXamarinWear.Models;
using ProjetXamarinWear.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

using Akavache;
using System.Reactive.Linq;

namespace ProjetXamarinWear.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;
        ItemsViewModel currentMessages;

        public ItemDetailPage(ItemDetailViewModel viewModel, ItemsViewModel currentMessage)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;

            this.currentMessages = currentMessage;
            testAlreadyOnFav(viewModel.Item.id,  this.currentMessages);

            changeButton();

        }

        private void testAlreadyOnFav(string id, ItemsViewModel fav) {
            viewModel.fav = false;
            foreach (Item m in fav.Save.ToList())
            {
                if (m.id == id) {
                    viewModel.fav = true;
                    break;
                }
            }


        }

        private void changeButton() {

            if (!viewModel.fav)
            {
                addF.IsVisible = true;
                removeF.IsVisible = false;
            }
            else
            {
                addF.IsVisible = false;
                removeF.IsVisible = true;
            }            
        }


        async void store() {
            await BlobCache.UserAccount.InsertObject("fav", currentMessages.Save);
        }

        private void addFav(object sender, EventArgs e)
        {
            viewModel.fav = true;

            currentMessages.Save.Add(viewModel.Item);

            store();

            changeButton();
        }

        private void removeFav(object sender, EventArgs e)
        {
            viewModel.fav = false;

            currentMessages.Items.Remove(viewModel.Item);
            currentMessages.Save.Remove(viewModel.Item);

            store();

            changeButton();
        }

    }
}