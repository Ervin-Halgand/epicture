using Imgur.API.Models;
using Imgur.API.Models.Impl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using epicture.Model;

namespace epicture
{
    /**
    * HomePage class.
    *
    * Controller of the View HomePage.Page that contain photo on your account.
    * Contain all method that modify the HomePage view
    */
    public sealed partial class HomePage : Page
    {
        /** \brief Model that contain API Information*/
        IApi imgurApi;

        /**
        * Constructor
        */
        public HomePage()
        {
            this.InitializeComponent();
        }

        /**
        * Get information from another page.
        */
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.imgurApi = e.Parameter as ImgurApi;
            ImagesLV.ItemsSource = await imgurApi.GetAccountImages();
            base.OnNavigatedTo(e);
        }

        /**
        * Button on the bottom of the page that change Page into upload page.
        */
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
           this.Frame.Navigate(typeof(UploadPage), imgurApi);
        }

        /**
        * Button on the bottom of the page that change Page into favorite page.
        */
        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
           this.Frame.Navigate(typeof(FavoritePage), imgurApi);
        }

        /**
        * Button on the bottom of the page that change Page into search page.
        */
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
           this.Frame.Navigate(typeof(SearchPage), imgurApi);
        }

        /**
        * Button (heart icon) that add/remove stuff in favorite.
        */
        private async void AddFavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            Button favButton = sender as Button;
            await imgurApi.AddFavorites(favButton.Tag.ToString());
            if (favButton.Content.ToString() == "\uEB52;")
                favButton.Content = "\uEB51;";
            else
                favButton.Content = "\uEB52;";
        }
    }
}
