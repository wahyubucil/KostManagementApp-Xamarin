using System;
using System.Collections.Generic;
using System.Text;
using Firebase.Database;
using Firebase.Database.Query;
using KostManagementApp.Models;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Firebase.Storage;

namespace KostManagementApp.Helpers
{
    public class FirebaseHelper
    {
        FirebaseClient firebase = new FirebaseClient("https://kost-management-app.firebaseio.com/");

        public async Task<List<Kost>> GetAllKosts()
        {
            var kostData = await firebase
                .Child("kosts")
                .OnceAsync<Kost>();

            var listKosts = new List<Kost>();

            if (kostData != null)
            {
                listKosts = kostData.Select(item => new Kost
                {
                    Name = item.Object.Name,
                    Address = item.Object.Address,
                    Description = item.Object.Description,
                    Rooms = item.Object.Rooms,
                    OwnerName = item.Object.OwnerName,
                    OwnerPhoneNumber = item.Object.OwnerPhoneNumber,
                    ImageUrl = item.Object.ImageUrl
                }).ToList();
            }

            return listKosts;
        }

        public async Task<List<KostData>> GetAllKostData()
        {
            var kostData = await firebase
                .Child("kosts")
                .OnceAsync<Kost>();

            var listKosts = new List<KostData>();

            if (kostData != null)
            {
                listKosts = kostData.Select(item => new KostData {
                    Id = item.Key,
                    Kost = new Kost
                    {
                        Name = item.Object.Name,
                        Address = item.Object.Address,
                        Description = item.Object.Description,
                        Rooms = item.Object.Rooms,
                        OwnerName = item.Object.OwnerName,
                        OwnerPhoneNumber = item.Object.OwnerPhoneNumber,
                        ImageUrl = item.Object.ImageUrl
                    }
                }).ToList();
            }

            return listKosts;
        }

        public async Task AddKost(Kost kost)
        {
            await firebase
                .Child("kosts")
                .PostAsync(kost);
        }

        public async Task UpdateKost(KostData kostData)
        {
            await firebase
                .Child("kosts")
                .Child(kostData.Id)
                .PutAsync(kostData.Kost);
        }

        public async Task DeleteKost(string id)
        {
            await firebase
                .Child("kosts")
                .Child(id)
                .DeleteAsync();
        }

        public async Task<String> StoreImage(Stream imageStream)
        {
            var storageImage = await new FirebaseStorage("kost-management-app.appspot.com")
                .Child("kost-images")
                .Child($"{Path.GetRandomFileName()}.jpg")
                .PutAsync(imageStream);

            return storageImage;
        }
    }
}
