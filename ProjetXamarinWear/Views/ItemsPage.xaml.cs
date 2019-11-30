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
using System.Net.Http;

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
            GetData();
            Device.StartTimer(TimeSpan.FromSeconds(30), () =>
            {
                var shouldTimerContinueWork = true;
                GetData();
                return shouldTimerContinueWork;
            });

           
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
                Boolean delete = true;
                foreach (Item m in viewModel.Items.ToList())
                {
                    foreach (Item mm in listeMessage) {
                        if (m.id == mm.id) {
                            delete = false;
                            break;                            
                        }                            
                    }
                    if (delete)
                        viewModel.Items.Remove(m);
                    delete = true;
                }

                Boolean ajout = true;
                foreach (Item m in listeMessage)
                {
                    foreach (Item mm in viewModel.Items.ToList())
                    {
                        if (m.id == mm.id)
                        {
                            ajout = false;
                            break;                            
                        }
                    }
                    if (ajout)
                        viewModel.Items.Insert(0, m);
                    ajout = true;
                }

                
            }, null);
            
            
        }
    }
}