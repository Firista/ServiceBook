using ServiceBook.DAL;
using ServiceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceBook.Helpers
{
    public class AdminHelper
    {
        public AdminHelper()
        {

        }
        public HttpCookie SetCookieForAdmin(Users Admin)
        {

            HttpCookie AdminCookie = new HttpCookie("Admin");
            var AdminData = GetAdminData(Admin.userId);

            AdminCookie["userId"] = Admin.userId.ToString();
            AdminCookie["email"] = AdminData.email.ToString();
            AdminCookie["name"] = AdminData.name.ToString();
            AdminCookie["surname"] = AdminData.surname;


            return AdminCookie;
        }
        public Users GetAdminData(int adminId)
        {

            using (var context = new ServiceBookContext())
            {
                var result = context.users.Select(x => x).Where(x => x.userId == adminId).FirstOrDefault();
                return result;
            }

        }
    }
}