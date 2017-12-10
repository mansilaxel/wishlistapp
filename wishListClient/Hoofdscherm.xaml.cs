using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
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
        }

        private async void ToonMijnLijsten(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            var json = await client.GetStringAsync(new Uri("http://localhost:51656/api/users/4/wishlists"));
            var lst = JsonConvert.DeserializeObject<ObservableCollection<WishList>>(json);
            lv.ItemsSource = lst;
            //Frame.Navigate(typeof(MijnLijsten));
        }

        private void ToonDeelnames(object sender, RoutedEventArgs e)
        {

        }

        private void ToonMijnCategorieen(object sender, RoutedEventArgs e)
        {

        }
    }
}
