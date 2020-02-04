using Imgur.API;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Enums;
using Imgur.API.Models;
using Imgur.API.Models.Impl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace epicture.Model
{
    /**
    * API used.
    *
    * ImgurApi is a child of IApi.
    * Contain own method to work.
    *
    */
    class ImgurApi : IApi
    {
        /** \brief User of the app*/
        ImgurClient client;
        OAuth2Endpoint oAuth2Endpoint;
        ImageEndpoint imageEndpoint;
        AccountEndpoint accountEndpoint;
        GalleryEndpoint galleryEndpoint;
        /** \brief Permit access to the user account and the API*/
        public static IOAuth2Token token;
        public static string code { get; set; }

        /**
        * Initialise Variable that permit access to the user account.
        */
        public ImgurApi()
        {
            this.client = new ImgurClient("76c0f5a4f7e671e", "fd6a836ec1237a2ba383941ca420b2c9bd63922f");
            this.oAuth2Endpoint = new OAuth2Endpoint(client);
            this.client.SetOAuth2Token(token);
            this.imageEndpoint = new ImageEndpoint(client);
            this.accountEndpoint = new AccountEndpoint(client);
            this.galleryEndpoint = new GalleryEndpoint(client);
        }

        /**
        * Get an Image from the Account.
        *
        * @param id ID of the image.
        */
        private async Task<IImage> GetImageFromId(string id)
        {
            try
            {
                var image = await imageEndpoint.GetImageAsync(id);
                return image;
            }
            catch (ImgurException imgurEx)
            {
                Debug.Write("An error occurred getting an image from Imgur.");
                Debug.Write(imgurEx.Message);
                return null;
            }
        }

        /**
        * Get all image in the user account.
        *
        * @param galleries Contain multiple stuff like image/gif....
        */
        private async Task<ObservableCollection<MyImage>> GetMyImages(IEnumerable<IGalleryItem> galleries)
        {
            ObservableCollection<MyImage> Images = new ObservableCollection<MyImage>();

            foreach (GalleryItem gallery in galleries)
            {
                var img = new MyImage();

                if (gallery.ToString() == "Imgur.API.Models.Impl.GalleryAlbum")
                {
                    var image = await this.GetImageFromId((gallery as GalleryAlbum).Cover);
                    img.Id = (gallery as GalleryAlbum).Cover;
                    if (image != null)
                    {
                        img.Source = image.Link;
                        if (image.Favorite == true)
                            img.Favorite = "\uEB52;";
                        else
                            img.Favorite = "\uEB51;";
                    }
                }
                if (gallery.ToString() == "Imgur.API.Models.Impl.GalleryImage")
                {
                    var image = await this.GetImageFromId((gallery as GalleryImage).Id);
                    img.Id = (gallery as GalleryImage).Id;
                    if (image != null)
                    {
                        img.Source = image.Link;
                        if (image.Favorite == true)
                            img.Favorite = "\uEB52;";
                        else
                            img.Favorite = "\uEB51;";
                    }
                }
                Images.Add(img);
            }
            return Images;
        }

        /**
        * Get the token that give access to the account.
        *
        * @param code code to obtain user information.
        */
        public static async Task<bool> GetTokenFromCode(string code)
        {
            var imgurApi = new ImgurApi();
            if (code != null)
                token = await imgurApi.oAuth2Endpoint.GetTokenByCodeAsync(code);
            return true;
        }

        async Task IApi.Connect()
        {
            var imgurApi = new ImgurApi();
            var authorizationUrl = imgurApi.oAuth2Endpoint.GetAuthorizationUrl(OAuth2ResponseType.Code);
            await Windows.System.Launcher.LaunchUriAsync(new Uri(authorizationUrl));
        }

        async Task IApi.AddFavorites(string img_ID)
        {
            await imageEndpoint.FavoriteImageAsync(img_ID);
        }

        async Task IApi.UploadImage(Windows.Storage.StorageFile file, string name, string description)
        {
            try
            {
                IImage image;
                using (var fs = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
                {
                    image = await imageEndpoint.UploadImageStreamAsync(fs.AsStream(), null, name, description);
                }
            }
            catch (ImgurException imgurEx)
            {
                Debug.Write("An error occurred uploading an image to Imgur.");
                Debug.Write(imgurEx.Message);
            }

        }

        async Task<IEnumerable<MyImage>> IApi.GetAccountImages()
        {
            var imgIds = await accountEndpoint.GetImageIdsAsync();
            ObservableCollection<MyImage> Images = new ObservableCollection<MyImage>();

            foreach (var imgId in imgIds)
            {
                var img = new MyImage();
                var image = await this.GetImageFromId(imgId);
                img.Source = image.Link;
                img.Id = imgId;
                if (image.Favorite == true)
                    img.Favorite = "\uEB52;";
                else
                    img.Favorite = "\uEB51;";
                Images.Add(img);
            }
            return Images;
        }

        async Task<IEnumerable<MyImage>> IApi.GetAccountFavorites()
        {
            var ret = "";
            HttpClient cli = new HttpClient();
            cli.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.TokenType, token.AccessToken);
            HttpResponseMessage response = await cli.GetAsync("https://api.imgur.com/3/account/" + token.AccountUsername + "/favorites");
            HttpContent content = response.Content;
            ret = await content.ReadAsStringAsync();
            var images = new List<MyImage>();
            if (ret != null)
            {
                var img = JsonConvert.DeserializeObject<RootObject>(ret);
                foreach (var elem in img.data)
                {
                    if (!elem.is_album)
                    {
                        var ga = new MyImage();
                        var image = await this.GetImageFromId(elem.id);
                        if (image.Favorite == true)
                        {
                            ga.Favorite = "\uEB52;";
                            ga.Id = image.Id;
                            ga.Source = image.Link;
                            images.Add(ga);
                        }
                    }

                }
            }
            return images;
        }

        async Task<IEnumerable<MyImage>> IApi.GetGalleryBySearch(string query)
        {
            return await this.GetMyImages(await this.galleryEndpoint.SearchGalleryAsync(query));
        }

    }
}
