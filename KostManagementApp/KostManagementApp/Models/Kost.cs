using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace KostManagementApp.Models
{
    public class Kost : BaseModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int Rooms { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhoneNumber { get; set; }
        public string ImageUrl { get; set; }

        public string Validate()
        {
            string[] props = { "Name", "Address", "OwnerName", "OwnerPhoneNumber" };
            return base.Validate(props);
        }
    }
}
