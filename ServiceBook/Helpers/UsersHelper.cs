using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceBook.Helpers
{
    public class UsersHelper
    {
        public int userId { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string roleName { get; set; }

        public UsersHelper(int userId, string name, string surname, string email, string password, string roleName)
        {
            this.userId = userId;
            this.name = name;
            this.surname = surname;
            this.email = email;
            this.password = password;
            this.roleName = roleName;

        }
    }
}