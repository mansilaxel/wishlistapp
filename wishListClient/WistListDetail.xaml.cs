using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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
    public sealed partial class WistListDetail : Page
    {
        public WistListDetail()
        {
            this.InitializeComponent();
        }


        private ObservableCollection<Wish> wishes;
        private HttpClient client = new HttpClient();
        WishList wishList = new WishList();
        private ObservableCollection<User> participants;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var wishlist = e.Parameter as WishList;
            this.wishList = wishlist;

            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Globals.LoggedInUser.access_token.ToString());
            var json = await client.GetStringAsync(new Uri("http://localhost:51656/api/WishLists/" + wishlist.Id + "/Wishes"));
            var lst = JsonConvert.DeserializeObject<ObservableCollection<Wish>>(json);
            this.wishes = lst;
            lv.ItemsSource = lst;

            var jsonParticipants = await client.GetStringAsync(new Uri("http://localhost:51656/api/WishLists/" + wishlist.Id + "/Participants"));
            var lstParticipants = JsonConvert.DeserializeObject<ObservableCollection<User>>(jsonParticipants);
            this.participants = lstParticipants;
            lvp.ItemsSource = lstParticipants;



        }

        private async void MakeWish(object sender, RoutedEventArgs e)
        {

            Wish wish = new Wish();
            wish.Title = "Nieuwke";

            var json = JsonConvert.SerializeObject(wish);


            var res = await client.PostAsync(new Uri("http://localhost:51656/api/WishList/" + wishList.Id + "/Wishes"),
                new StringContent(json, Encoding.UTF8, "application/json"));

            var jsonResp = await res.Content.ReadAsStringAsync();
            var nieuwke = JsonConvert.DeserializeObject<Wish>(jsonResp);

            wishes.Add(nieuwke);

        }
        private async void Delete(object sender, RoutedEventArgs e)
        {
            if (lv.SelectedItem != null)
            {

                var selectie = lv.SelectedItem as Wish;

                var res = await client.DeleteAsync(new Uri("http://localhost:51656/api/Wishes/" + selectie.Id));
                if (res.IsSuccessStatusCode)
                {
                    wishes.Remove(selectie);
                }

            }


        }
  
        private async void Show(object sender, RoutedEventArgs e)
        {

            if (lv.SelectedItem != null)
            {

                var selectie = lv.SelectedItem as Wish;

                Frame.Navigate(typeof(WishDetail), selectie);

            }
        }
        private async void DeleteParticipant(object sender, RoutedEventArgs e)
        {
            if (lvp.SelectedItem != null)
            {

                var selectie = lvp.SelectedItem as User;
                
                


                var res = await client.DeleteAsync(new Uri("http://localhost:51656/api/WishLists/ParticipantToRemove/" + selectie.Id));
                if (res.IsSuccessStatusCode)
                {
                    participants.Remove(selectie);
                }

            }


        }

        private async void AddParticipant(object sender, RoutedEventArgs e)
        {
            ParticipantOnWishList pown = new ParticipantOnWishList();
            pown.WishList = this.wishList;
            pown.WishListId = this.wishList.Id;

            var json = JsonConvert.SerializeObject(pown);
            var email = ParticipantEmail.Text;
          
            var res = await this.client.PostAsync(new Uri("http://localhost:51656/api/WishLists/ParticipantToAdd/" + email),
                new StringContent(json, Encoding.UTF8, "application/json"));

            var jsonResp = await res.Content.ReadAsStringAsync();
            var nieuwke = JsonConvert.DeserializeObject<ParticipantOnWishList>(jsonResp);


            this.participants.Add(nieuwke.User);

        }

    }
}
