using Newtonsoft.Json;
using ProjetXamarinWear.Models;
using ProjetXamarinWear.ViewModels;
using Syncfusion.SfMaps.XForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjetXamarinWear.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class AboutPage : ContentPage
    {

        SfMaps map;
        public AboutPage()
        {
            InitializeComponent();

            this.map = this.FindByName<SfMaps>("carte");

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
                    MapMarker marker = new MapMarker();
                    marker.Label = m.student_message;
                    marker.Latitude = m.gps_lat.ToString();
                    marker.Longitude = m.gps_long.ToString();
                    map.Layers[0].Markers.Add(marker);
                }
            }, null);


        }
    }
}