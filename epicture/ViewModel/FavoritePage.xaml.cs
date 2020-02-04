using epicture.Model;
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
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace epicture
{

    /**
    * FavoritePage class.
    *
    * Controller of the View FavoritePage.Page that contain all of the favorite in the account, also permit to unfav them.
    * Contain all method that modify the FavoritePage view
    */
    public sealed partial class FavoritePage : Page
    {
        /** \brief Model that contain API Information*/
        IApi imgurApi; 

        /**
        * Constructor
        */
        public FavoritePage()
        {
            this.InitializeComponent();
        }


        /**
        * Get information from another page.
        */
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.imgurApi = e.Parameter as ImgurApi;
            ImagesLV.ItemsSource = await imgurApi.GetAccountFavorites();
            base.OnNavigatedTo(e);
        }

        /**
        * Button (heart icon) that add/remove stuff in favorite.
        */
        private async void AddFavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            Button favButton = (sender as Button);
            await imgurApi.AddFavorites(favButton.Tag.ToString());
            if (favButton.Content.ToString() == "\uEB52;")
                favButton.Content = "\uEB51;";
            else
                favButton.Content = "\uEB52;";
            ImagesLV.ItemsSource = await imgurApi.GetAccountFavorites();
        }

        /**
        * Button on the bottom of the page that change Page into upload page.
        */
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UploadPage), imgurApi);
        }

        /**
        * Button on the bottom of the page that change Page into search page.
        */
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SearchPage), imgurApi);
        }

        /**
        * Button on the bottom of the page that change Page into my photo page.
        */
        private void PhotoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage), imgurApi);
        }
    }
}
