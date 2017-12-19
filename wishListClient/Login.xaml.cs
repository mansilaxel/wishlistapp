using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserService userService = new UserService();
            var user = userService.AuthenticateUser(email.Text, pw.Text);            

            if (user == null)
            {
                new MessageDialog("Aanmelden mislukt").ShowAsync();
                return;
            }
            User loggedInUser = userService.GetUserDetails(user);
            loggedInUser.access_token = user.access_token;
            Globals.LoggedInUser = loggedInUser;            
            new MessageDialog("Welkom " + loggedInUser.FirstName).ShowAsync();
            Frame.Navigate(typeof(Hoofdscherm));

        }
    }
}
