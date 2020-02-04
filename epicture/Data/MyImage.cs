using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace epicture
{
    class MyImage
    {
        public string Source { get; set; }
        public string Id { get; set; }
        public string Favorite { get; set; }
    }

    public class Datum
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string cover { get; set; }
        public int cover_width { get; set; }
        public int cover_height { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string account_url { get; set; }
        public int account_id { get; set; }
        public string privacy { get; set; }
        public int views { get; set; }
        public string link { get; set; }
        public int ups { get; set; }
        public int downs { get; set; }
        public int points { get; set; }
        public int score { get; set; }
        public bool is_album { get; set; }
        public string vote { get; set; }
        public bool favorite { get; set; }
        public bool nsfw { get; set; }
        public object section { get; set; }
        public int comment_count { get; set; }
        public int favorite_count { get; set; }
        public string topic { get; set; }
        public string topic_id { get; set; }
        public int images_count { get; set; }
        public object datetime { get; set; }
        public bool in_gallery { get; set; }
        public bool in_most_viral { get; set; }
        public object tags { get; set; }
        public object images { get; set; }
        public bool has_sound { get; set; }
        public bool animated { get; set; }
        public string type { get; set; }
        public int size { get; set; }
    }

    public class RootObject
    {
        public int status { get; set; }
        public bool success { get; set; }
        public List<Datum> data { get; set; }
    }
}
