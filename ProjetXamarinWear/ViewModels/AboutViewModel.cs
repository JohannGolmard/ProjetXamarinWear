using System;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace ProjetXamarinWear.ViewModels
{
    public class AboutViewModel : ContentPage
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
            Map map = new Map();
            Content = map;
        }

        public ICommand OpenWebCommand { get; }
    }
}