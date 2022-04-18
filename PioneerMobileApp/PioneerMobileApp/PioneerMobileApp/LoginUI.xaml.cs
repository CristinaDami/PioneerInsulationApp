using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PioneerMobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginUI : ContentPage
    {
        public LoginUI()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) && string.IsNullOrEmpty(txtPassword.Text))
            {
                DisplayAlert("Ops...", "Username and Password cannot be empty!", "Ok");
            }
            else if (txtUsername.Text == "Admino" && txtPassword.Text == "Admino")
            {
                Navigation.PushAsync(new HomePage());
            }
            else if (txtUsername.Text == "Adminf" && txtPassword.Text == "Adminf")
            {
                Navigation.PushAsync(new HomePage());
            }
            else if (txtUsername.Text == "Ope1" && txtPassword.Text == "Ope1")
            {
                Navigation.PushAsync(new HomePage());
            }
            else
            {
                DisplayAlert("Ops...", "Username or Password not correct!", "Ok");
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterPage());
        }
    }
}