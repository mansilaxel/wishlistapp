using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
using wishListClient.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace wishListClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Hoofdscherm : Page
    {
        public Hoofdscherm()
        {
            this.InitializeComponent();
            //UserService userService = new UserService();
            //User authUser = userService.GetUserDetails(Globals.LoggedInUser);
            //if(authUser == null)
            //{
                
            //    Frame.Navigate(typeof(MainPage));
            //}
            //Globals.LoggedInUser = authUser;

        }

        private void ToonMijnLijsten(object sender, RoutedEventArgs e)
        {
            //HttpClient client = new HttpClient();
            //var current = Globals.LoggedInUser;
            //var id = current.Id;
            //signed in user(later invullen) ipv hardcode.
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Globals.LoggedInUser.access_token.ToString());
            //var json = await client.GetStringAsync(new Uri("http://localhost:51656/api/MyWishLists"));
            //var lst = JsonConvert.DeserializeObject<ObservableCollection<WishList>>(json);
            //lv.ItemsSource = lst;
            Frame.Navigate(typeof(MijnLijsten));
        }

        private async void ToonDeelnames(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Globals.LoggedInUser.access_token.ToString());
            var json = client.GetStringAsync(new Uri("http://localhost:51656/api/ParticipatingOnWishLists")).Result;
            var lst = JsonConvert.DeserializeObject<ObservableCollection<WishList>>(json);
            lv.ItemsSource = lst;


        }

        private async void ToonMijnCategorieen(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Globals.LoggedInUser.access_token.ToString());
            var json = client.GetStringAsync(new Uri("http://localhost:51656/api/MyWishCategories")).Result;
            var lst = JsonConvert.DeserializeObject<ObservableCollection<WishCategory>>(json);
            lv.ItemsSource = lst;
        }
    }
}
