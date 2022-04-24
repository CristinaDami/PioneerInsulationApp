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
        private List<PioneerUser> _pioneerUsers;
        private PioneerRepository _repository;

        public LoginUI()
        {
            InitializeComponent();

            _repository = new PioneerRepository();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            // Setting internal variable and trimming eventual spaces
            var userName = txtUsername.Text.Trim();
            var password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(password))
            {
                DisplayAlert("Ops...", "Username and Password cannot be empty!", "Ok");
                return;
            }

            var pioneerUser = _repository.GetUser(userName, password);

            if (pioneerUser != null)
            {
                Task.Run(() => SecureStorage.SetAsync(ApplicationConstants.CurrentUser, JsonConvert.SerializeObject(pioneerUser)));
                UserCallToAction(pioneerUser);
            }
            else
            {
                DisplayAlert("Ops...", "User not found! Please check you Username and Password.", "Ok");
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