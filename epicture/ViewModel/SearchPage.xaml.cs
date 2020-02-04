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
    * SearchPage class.
    *
    * Controller of the View SearchPage.Page for searching stuff.
    * Contain all method that modify the SearchPage view
    */
    public sealed partial class SearchPage : Page
    {
        /** \brief Model that contain API Information*/
        IApi imgurApi;

        /**
        * Constructor
        */
        public SearchPage()
        {
            this.InitializeComponent();
        }

        /**
        * Get information from another page.
        */
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.imgurApi = e.Parameter as ImgurApi;
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
        * Button on the bottom of the page that change Page into Favorite page.
        */
        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FavoritePage), imgurApi);
        }

        /**
        * Button on the bottom of the page that change Page into my photo page.
        */
        private void PhotoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage), imgurApi);
        }

        /**
        * TextBox that search stuff after input and validation (enter).
        */
        private void SearchTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e != null && e.Key == Windows.System.VirtualKey.Enter)
                SearchTextBox_QuerySubmitted(sender as SearchBox, null);
        }

        /**
        * button that validate search.
        */
        private async void SearchTextBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            if (SearchTextBox.QueryText != null && SearchTextBox.QueryText != "")
                ImagesLV.ItemsSource = await imgurApi.GetGalleryBySearch(SearchTextBox.QueryText);
        }

        /**
        * Button (heart icon) that add/remove stuff in favorite.
        */
        private async void AddFavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            Button favButton = (Button)sender;
            if (favButton.Tag != null)
                await imgurApi.AddFavorites(favButton.Tag.ToString());
            if (favButton.Content != null && favButton.Content.ToString() == "\uEB52;")
                favButton.Content = "\uEB51;";
            else
                favButton.Content = "\uEB52;";
        }
    }
}
