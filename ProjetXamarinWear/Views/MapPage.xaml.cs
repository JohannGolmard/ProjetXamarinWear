using Newtonsoft.Json;
using ProjetXamarinWear.Models;
using ProjetXamarinWear.ViewModels;
using Syncfusion.SfMaps.XForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjetXamarinWear.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MapPage : ContentPage
    {

        SfMaps map;
        ItemsViewModel viewModel;
        public MapPage()
        {
            InitializeComponent();

            this.map = this.FindByName<SfMaps>("carte");
            viewModel = new ItemsViewModel();

            Device.StartTimer(TimeSpan.FromSeconds(30), () =>
            {
                var shouldTimerContinueWork = true;
                GetData();
                return shouldTimerContinueWork;
            });

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
                    foreach (Item mm in listeMessage)
                    {
                        if (m.id == mm.id)
                        {
                            delete = false;
                            break;
                        }
                    }
                    if (delete)
                    {
                        foreach (MapMarker marker in map.Layers[0].Markers.ToList())
                        {
                            if (marker.Label == m.student_message && marker.Latitude == m.gps_lat.ToString() && marker.Longitude == m.gps_long.ToString()) {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    map.Layers[0].Markers.Remove(marker);
                                });
                                
                                break;
                            }
                        }  
                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        viewModel.Items.Remove(m);
                    });
                    
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
                    if (ajout) {

                        MapMarker marker = new MapMarker();
                        marker.Label = m.student_message;
                        marker.Latitude = m.gps_lat.ToString();
                        marker.Longitude = m.gps_long.ToString();

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            map.Layers[0].Markers.Add(marker);
                        });

                    }
                    ajout = true;
                }
                
            }, null);


        }       
    }
}