using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using KostManagementApp.Helpers;
using KostManagementApp.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Diagnostics;
using XF.Material.Forms.UI.Dialogs;

namespace KostManagementApp
{
    public partial class KostEntryPage : ContentPage
    {
        readonly FirebaseHelper firebaseHelper = new FirebaseHelper();
        MediaFile file;
        bool newData = false;

        public KostEntryPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var kostData = (KostData)BindingContext;
            if (string.IsNullOrWhiteSpace(kostData.Id))
            {
                newData = true;
            }

            if (newData)
            {
                InputRooms.Text = "";
                ButtonGrid.Children.Remove(ButtonDelete);
                Grid.SetColumnSpan(ButtonSubmit, 2);
                KostEntryLayout.Children.Remove(ButtonDeleteImage);
                InputPhoto.Source = ImageSource.FromResource("KostManagementApp.Assets.placeholder_image.png");
            }
            else
            {
                InputPhoto.Source = ImageSource.FromUri(new Uri(kostData.Kost.ImageUrl));
                PickPhoto.IsVisible = false;
            }
        }

        private async void PickPhoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            try
            {
                file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });
                if (file == null) return;

                InputPhoto.Source = ImageSource.FromStream(() =>
                {
                    var imageStream = file.GetStream();
                    return imageStream;
                });
            } catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void ButtonDeleteImage_Clicked(object sender, EventArgs e)
        {
            InputPhoto.Source = ImageSource.FromResource("KostManagementApp.Assets.placeholder_image.png");
            PickPhoto.IsVisible = true;
            KostEntryLayout.Children.Remove(ButtonDeleteImage);
        }

        private async void ButtonSubmit_Clicked(object sender, EventArgs e)
        {
            var kostData = (KostData)BindingContext;
            var kost = kostData.Kost;

            string alertMessage;

            var error = kost.Validate();
            if (error != null)
            {
                await DisplayAlert("Error", error, "Try Again");
                return;
            }

            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "On Processing :)");

            if (newData)
            {
                if (file == null)
                {
                    await DisplayAlert("Error", "Image can't be blank", "Try Again");
                    return;
                }

                var imageUrl = await firebaseHelper.StoreImage(file.GetStream());
                kost.ImageUrl = imageUrl;
                await firebaseHelper.AddKost(kost);
                alertMessage = "Kost Added Successfully";
            }
            else
            {
                if (file != null)
                {
                    var imageUrl = await firebaseHelper.StoreImage(file.GetStream());
                    kost.ImageUrl = imageUrl;
                }
                await firebaseHelper.UpdateKost(kostData);
                alertMessage = "Kost Updated Successfully";
            }

            await loadingDialog.DismissAsync();

            await DisplayAlert("Success", alertMessage, "OK");
            await Navigation.PopAsync();
        }

        private async void ButtonDelete_Clicked(object sender, EventArgs e)
        {
            var kostData = (KostData)BindingContext;
            await firebaseHelper.DeleteKost(kostData.Id);
            await DisplayAlert("Success", "Kost Deleted Successfully", "OK");
            await Navigation.PopAsync();
        }

        private async void InputRooms_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (InputRooms.Text.Length < 1) return;

            try
            {
                var newNumber = Convert.ToUInt32(e.NewTextValue);
                if (newNumber < 1) InputRooms.Text = "";
            }
            catch (FormatException formatException)
            {
                InputRooms.Text = e.OldTextValue;
            }
            catch (OverflowException overflowException)
            {
                await DisplayAlert("Error", "Number is too much", "Try Again");
                InputRooms.Text = e.OldTextValue;
            }
        }
    }
}