using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace KostManagementApp.Models
{
    public partial class BaseModel
    {
        public string Validate(string[] props)
        {
            string errorMessage = null;

            foreach (string prop in props)
            {
                PropertyInfo property = this.GetType().GetProperty(prop);
                string value = property.GetValue(this) == null ? "" : property.GetValue(this).ToString().Trim();
                if (string.IsNullOrEmpty(value))
                {
                    errorMessage = prop + " can't be blank";
                    break;
                }
            }

            return errorMessage;
        }
    }
}
