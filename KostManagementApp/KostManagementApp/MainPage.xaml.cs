using System;
using System.Linq;
using Xamarin.Forms;
using KostManagementApp.Helpers;
using KostManagementApp.Models;
using System.Collections.Generic;
using Firebase.Database;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace KostManagementApp
{
    public partial class MainPage : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        FirebaseClient firebase = new FirebaseClient("https://kost-management-app.firebaseio.com/");

        ObservableCollection<KostData> allKostData = new ObservableCollection<KostData>();

        public MainPage()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.WPF) ToolbarItems.Remove(AddKostMenu);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetData();
            /* listKosts.ItemsSource = allKostData;
            var observable = firebase
                .Child("kosts")
                .AsObservable<Kost>()
                .Subscribe(d =>
                {
                    var index = allKostData.IndexOf(allKostData.FirstOrDefault(i => i.Id == d.Key));
                    if (d.EventType.ToString() == "InsertOrUpdate")
                    {
                        if (index > -1)
                        {
                            // Update
                            allKostData[index].Kost = d.Object;
                        }
                        else
                        {
                            // Insert
                            allKostData.Add(new KostData
                            {
                                Id = d.Key,
                                Kost = d.Object
                            });
                        }
                    }
                    else if (d.EventType.ToString() == "Delete")
                    {
                        allKostData.RemoveAt(index);
                    }
                }); */
        }

        private async void GetData()
        {
            listKosts.BeginRefresh();
            var allKostData = await firebaseHelper.GetAllKostData();
            listKosts.ItemsSource = allKostData;
            listKosts.EndRefresh();
        }

        private void Refresh_Clicked(object sender, EventArgs e)
        {
            GetData();
        }

        private async void AddKostMenu_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new KostEntryPage
            {
                BindingContext = new KostData
                {
                    Id = null,
                    Kost = new Kost()
                }
            });
        }

        private async void OnListKostsItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;

            if (Device.RuntimePlatform == Device.WPF)
            {
                await Navigation.PushAsync(new KostDetailPage
                {
                    BindingContext = e.SelectedItem as KostData
                });
            }
            else
            {
                await Navigation.PushAsync(new KostEntryPage
                {
                    BindingContext = e.SelectedItem as KostData
                });
            }
        }
    }
}
