using PioneerMobileApp.Models;
using PioneerMobileApp.Navigation;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PioneerMobileApp
{
    using static PioneerMobileApp.Helpers.EnumHelpers;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        public HomePage(PioneerUser pioneerUser)
        {
            InitializeComponent();

            Label userType = this.FindByName<Label>("lblUserType");
            
            userType.Text = $"UserType: {pioneerUser.UserTypeId.GetEnumDescription()}";

            Label lblWelcome = this.FindByName<Label>("lblWelcome");
            lblWelcome.Text = $"Welcome back {pioneerUser.FirstName} {pioneerUser.LastName}\nto Pioneer Homepage!";
        }
        
        private void Button_JobsClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Jobs());
        }

        private void Button_TimesheetClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Timesheet());
        }

        private void Button_ProfileClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Profile());
        }
    }
}