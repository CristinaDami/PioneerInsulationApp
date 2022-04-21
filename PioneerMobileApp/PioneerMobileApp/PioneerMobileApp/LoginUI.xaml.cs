using PioneerMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PioneerMobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginUI : ContentPage
    {
        private List<PioneerUser> _pioneerUsers;

        public LoginUI()
        {
            InitializeComponent();

            _pioneerUsers = new List<PioneerUser>
            {
                new PioneerUser("Cristina", "Damian", "A", "A", UserType.Admin, "Boss"),
                new PioneerUser("Moses", "Damian", "Admina", "Admina", UserType.AdminOffice, "Administration"),
                new PioneerUser("Bogdan", "Popescu", "Adminu", "Adminu", UserType.Operative, "Operative"),
            };
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) && string.IsNullOrEmpty(txtPassword.Text))
            {
                DisplayAlert("Ops...", "Username and Password cannot be empty!", "Ok");
                return;
            }

            var pioneerUser =  _pioneerUsers.SingleOrDefault(x => x.UserName.Equals(txtUsername.Text) && x.Password.Equals(txtPassword.Text));

            if (pioneerUser != null)
            {
                UserCallToAction(pioneerUser);
            }
            else
            {
                DisplayAlert("Ops...", "Username or Password not correct!", "Ok");
                return;
            }          
        }

        private void UserCallToAction(PioneerUser pioneerUser)
        {
            var hp = new HomePage(pioneerUser)
            {
                BindingContext = pioneerUser
            };
            Navigation.PushAsync(hp);
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterPage());
        }
    }
}