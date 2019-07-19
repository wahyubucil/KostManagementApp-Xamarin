using System;
using System.Collections.Generic;
using System.Text;

namespace KostManagementApp.Models
{
    public class KostData : BaseModel
    {
        public string Id { get; set; }

        public Kost Kost { get; set; }
    }
}
