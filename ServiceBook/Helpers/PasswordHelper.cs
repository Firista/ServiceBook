using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace ServiceBook.Helpers
{
    public class PasswordHelper
    {
        public string GeneratePassword(string password)
        {
            var hashed2 = Crypto.SHA256(password);
            var newPassword = hashed2;
            return newPassword;
        }
    }
}