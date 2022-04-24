using Newtonsoft.Json;
using PioneerMobileApp.Common;
using PioneerMobileApp.Models;
using PioneerMobileApp.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PioneerMobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginUI : ContentPage
    {
        private readonly PioneerRepository _repository;

        public LoginUI()
        {
            InitializeComponent();

            // Pioneer database repository access layer
            _repository = new PioneerRepository();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            // Setting internal variable and trimming eventual spaces
            var userName = txtUsername.Text.Trim();
            var password = txtPassword.Text.Trim();

            // Checking if user name and password are not null or empty
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(password))
            {
                DisplayAlert("Ops...", "Username and Password cannot be empty!", "Ok"); // Returns error
                return;
            }

            var pioneerUser = _repository.GetUser(userName, password);// Fetch User from the database

            if (pioneerUser != null)
            {
                // Storing securely PioneerUser into SecureStorage (serialized as a JSON string)
                Task.Run(() => SecureStorage.SetAsync(ApplicationConstants.CurrentUser, JsonConvert.SerializeObject(pioneerUser)));

                UserCallToAction(pioneerUser); // Redirect to the Main menu 
            }
            else
            {
                // User has not been found in the database
                DisplayAlert("Ops...", "User not found! Please check you Username and Password.", "Ok"); // Returns error
                return;
            }          
        }

        private void UserCallToAction(PioneerUser pioneerUser)
        {
            var homePage = new HomePage(pioneerUser)
            {
                BindingContext = pioneerUser
            };
            Navigation.PushAsync(homePage); // User authenticated and redirected to the Main menu 
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterPage());
        }
    }
}