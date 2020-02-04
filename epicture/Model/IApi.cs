using Imgur.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace epicture.Model
{
/**
 * Interface of API.
 *
 * IApi contain all function that Epicture app need to connect to the wished API.
 * It also contain method to display content.
 *
 */
    interface IApi
    {
        /**
        * Get access to your API account.
        */
        Task Connect();
        /**
        * Add stuff as favorite to your API account.
        *
        * @param img_ID Contain the ID of th stuff to add.
        */
        Task AddFavorites(string img_ID);
        /**
        * Add stuff to your API account.
        *
        * @param file Contain the stuff to upload.
        * @param name Contain the title of the file to upload.
        * @param description Contain a description of the stuff to upload.
        */
        Task UploadImage(StorageFile file, string name, string description);
        /**
        * Get all Favorite in your API account.
        */
        Task<IEnumerable<MyImage>> GetAccountFavorites();
        /**
        * Get all Image in your API account.
        */
        Task<IEnumerable<MyImage>> GetAccountImages();
        /**
        * Search stuff on the API.
        *
        * @param query Contain wanted research keywords.
        */
        Task<IEnumerable<MyImage>> GetGalleryBySearch(string query);
    }
}
