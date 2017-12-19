using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
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
using Newtonsoft.Json;
using Windows.UI.Core;
using System.Net.Http.Headers;
using System.Text;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace wishListClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MijnLijsten : Page
    {
        public MijnLijsten()
        {
            this.InitializeComponent();            
        }

        private ObservableCollection<WishList> wishlists;
        private HttpClient client = new HttpClient();


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Globals.LoggedInUser.access_token.ToString());
            var json = await client.GetStringAsync(new Uri("http://localhost:51656/api/MyWishLists"));
            var lst = JsonConvert.DeserializeObject<ObservableCollection<WishList>>(json);
            this.wishlists = lst;
            lv.ItemsSource = lst;
            


        }

        private async void MakeNewList(object sender, RoutedEventArgs e)
        {

            WishList wishlist = new WishList();
            wishlist.Name = "tst";

            var json = JsonConvert.SerializeObject(wishlist);

            var res = await client.PostAsync(new Uri("http://localhost:51656/api/WishLists"),
                new StringContent(json, Encoding.UTF8, "application/json"));

            var jsonResp = await res.Content.ReadAsStringAsync();
            var nieuwke = JsonConvert.DeserializeObject<WishList>(jsonResp);

            wishlists.Add(nieuwke);

        }
    }
}
