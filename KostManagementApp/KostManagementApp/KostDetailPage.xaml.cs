using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using KostManagementApp.Models;
using KostManagementApp.Helpers;
using System.Diagnostics;

namespace KostManagementApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KostDetailPage : ContentPage
    {
        readonly FirebaseHelper firebaseHelper = new FirebaseHelper();
        public KostDetailPage()
        {
            InitializeComponent();
        }

        private async void ButtonDelete_Clicked(object sender, EventArgs e)
        {
            var kostData = (KostData)BindingContext;
            await firebaseHelper.DeleteKost(kostData.Id);
            await DisplayAlert("Success", "Kost Deleted Successfully", "OK");
            await Navigation.PopAsync();
        }
    }
}