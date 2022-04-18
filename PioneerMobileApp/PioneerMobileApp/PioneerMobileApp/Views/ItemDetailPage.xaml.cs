using PioneerMobileApp.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace PioneerMobileApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}