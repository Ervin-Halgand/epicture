using epicture.Model;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace epicture
{
    /**
    * MainPage class.
    *
    * Controller of the View MainPage.First page of the app that permit connection to the API.
    * Contain all method that modify the FavoritePage view
    */
    public sealed partial class MainPage : Page
    {
        /** \brief Initialisation of the model that contain API Information*/
        IApi imgurApi = new ImgurApi();

        /**
        * Constructor
        */
        public MainPage()
        {
            this.InitializeComponent();
        }

        /**
        * Button that connect user to the account
        */
        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            await imgurApi.Connect();
        }
    }
}
