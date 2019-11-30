using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ProjetXamarinWear.Models;
using ProjetXamarinWear.Views;
using ProjetXamarinWear.ViewModels;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace ProjetXamarinWear.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
            viewModel.Items.Clear();

            GetData();
        }


        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private void RefreshData(object sender, EventArgs e)
        {

            viewModel.Items.Clear();
            GetData();

        }
               
        private void GetData()
        {

            string testUri = "https://hmin309-embedded-systems.herokuapp.com/message-exchange/messages/";
            HttpWebRequest request = WebRequest.CreateHttp(testUri);
            request.Method = WebRequestMethods.Http.Get;

            request.BeginGetResponse((arg) =>
            { 
                Stream stream = request.EndGetResponse(arg).GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                string response = sr.ReadToEnd();
                var listeMessage = JsonConvert.DeserializeObject<List<Item>>(response);
                foreach (Item m in listeMessage)
                {
                    viewModel.Items.Add(m);
                }
            }, null);
            
            
        }
    }
}