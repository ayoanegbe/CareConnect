using CareConnect.Mobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace CareConnect.Mobile.Views
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