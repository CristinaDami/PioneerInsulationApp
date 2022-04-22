using Newtonsoft.Json;
using PioneerMobileApp.Common;
using PioneerMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

            pioneerUser.SetEvents((DateTime.Now, new List<EventModel>() { new EventModel { Name = "Event1", Description = "Description1" } }));
            pioneerUser.SetEvents((DateTime.Now.AddDays(1), new List<EventModel>() { new EventModel { Name = "Event2", Description = "Description2" } }));
            pioneerUser.SetEvents((DateTime.Now.AddDays(2), new List<EventModel>() { new EventModel { Name = "Event3", Description = "Description3" } }));

            if (pioneerUser != null)
            {
                Task.Run(() => SecureStorage.SetAsync(ApplicationConstants.CurrentUser, JsonConvert.SerializeObject(pioneerUser)));
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