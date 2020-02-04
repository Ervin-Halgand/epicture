using System;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using epicture.Model;

namespace epicture
{
    /**
    * UploadPage class.
    *
    * Controller of the View UploadPage.Page for uploading stuff on your account.
    * Contain all method that modify the UploadPage view
    */
    public sealed partial class UploadPage : Page
    {
        /** \brief Model that contain API Information*/
        IApi imgurApi;
        /** \brief Stuff to upload*/
        StorageFile file = null;
        /** \brief title of the stuff*/
        string imageName = null;
        /** \brief description of the stuff*/
        string imageDescription = null;

        /**
        * Constructor
        */
        public UploadPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.imgurApi = e.Parameter as ImgurApi;
            base.OnNavigatedTo(e);
        }

        /**
        * Get information from another page.
        */
        private async void ChooseImage_Button_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".gif");

            file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                using (var Stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    var result = new BitmapImage();
                    await result.SetSourceAsync(Stream);
                    ImageToUpload.Source = result;
                }
                RemoveButton.Visibility = Visibility.Visible;
                ChooseImage.Visibility = Visibility.Collapsed;
            }
        }

        /**
        * Get input of title TextBox.
        */
        private void UploadName_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            imageName = Name.Text;
        }

        /**
        * Get input of description TextBox.
        */
        private void UploadDescription_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            imageDescription = Description.Text;
        }

        /**
        * Button that submit stuff on your account.
        */
        private async void UploadSubmit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (file != null)
            {
                await imgurApi.UploadImage(file, imageName, imageDescription);
                var messageDialog = new MessageDialog(file.Name + " was Uploaded on Imgur");
                await messageDialog.ShowAsync();
                ChooseImage.Visibility = Visibility.Visible;
                RemoveButton.Visibility = Visibility.Collapsed;
                file = null;
                ImageToUpload.Source = null;
                imageDescription = null;
                imageName = null;
            }
            else
            {
                var messageDialog = new MessageDialog("Choose an image to upload.");
                await messageDialog.ShowAsync();
            }
        }

        /**
        * Button that remove choosen image that permit to chose another image.
        */
        private void RemoveImage_Button_Click(object sender, RoutedEventArgs e)
        {
            ChooseImage.Visibility = Visibility.Visible;
            RemoveButton.Visibility = Visibility.Collapsed;
            ImageToUpload.Source = null;
            file = null;
        }

        /**
        * Button on the bottom of the page that change Page into my photo page.
        */
        private void PhotoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage), imgurApi);
        }

        /**
        * Button on the bottom of the page that change Page into Favorite page.
        */
        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
           this.Frame.Navigate(typeof(FavoritePage), imgurApi);
        }

        /**
        * Button on the bottom of the page that change Page into Search page.
        */
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
           this.Frame.Navigate(typeof(SearchPage), imgurApi);
        }
    }
}
