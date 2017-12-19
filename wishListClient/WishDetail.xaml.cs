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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace wishListClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WishDetail : Page
    {
        public WishDetail()
        {
            this.InitializeComponent();
            
        }

        private HttpClient client = new HttpClient();
        Wish wish = new Wish();
        List<WishCategory> myWishCategories = new List<WishCategory>();
        


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var wish = e.Parameter as Wish;
                       
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Globals.LoggedInUser.access_token.ToString());
            var jsonwish = client.GetStringAsync(new Uri("http://localhost:51656/api/Wishes/" + wish.Id)).Result;
            var wishdb = JsonConvert.DeserializeObject<Wish>(jsonwish);
            this.wish = wishdb;

            OldName.Text = wishdb.Title;
            if(wishdb.WishCategory != null) {OldCat.Text = wishdb.WishCategory.Name; }
            if (wishdb.Description != null) { OldDesc.Text = wishdb.Description; }
                
            


            //api/MyWishCategories
            var json = await client.GetStringAsync(new Uri("http://localhost:51656/api/MyWishCategories"));
            var lst = JsonConvert.DeserializeObject<List<WishCategory>>(json);
            myWishCategories = lst;



            foreach (var x in lst)
            {
                WishCategoryCB.Items.Add(x.Name);
            }




        }


        private async void Edit(object sender, RoutedEventArgs e)
        {

            wish.Title = Name.Text;
            wish.Description = Description.Text;

            var addingthisone = myWishCategories.FirstOrDefault(x => x.Name.Equals(WishCategoryCB.SelectedItem));
            wish.WishCategory = addingthisone;

                var json = JsonConvert.SerializeObject(wish);

                var res = await client.PutAsync(new Uri("http://localhost:51656/api/Wishes/" + wish.Id),
                    new StringContent(json, Encoding.UTF8, "application/json"));
            if (res.IsSuccessStatusCode)
            {
                Frame rootFrame = Window.Current.Content as Frame;
                
                rootFrame.GoBack();
            }

        }
    }
}
