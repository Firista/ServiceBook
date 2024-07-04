using ServiceBook.DAL;
using ServiceBook.Helpers;
using ServiceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ServiceBook.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {

        public ActionResult Login()
        {

            if (User.IsInRole("Customer"))
            {
                return RedirectToAction("Index", "Customer");
            }
            else if (User.IsInRole("Workshop"))
            {
                return RedirectToAction("Index", "VeheliceWorkShop");
            }
            else if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(Users user)
        {
            HttpCookie userCookie = new HttpCookie("UserBegining");
            userCookie.Value = user.userId.ToString();
            userCookie.Expires = DateTime.Now.AddDays(30);
            userCookie["email"] = user.email.ToString();
            Response.Cookies.Add(userCookie);
            Response.SetCookie(userCookie);

            PasswordHelper haslo = new PasswordHelper();
            user.password = haslo.GeneratePassword(user.password);

            var U = new ServiceBookContext();
            var count = U.users.Where(x => x.email == user.email && x.password == user.password).FirstOrDefault();
            if (count == null)
            {
                ViewBag.Msg = "Invalid User";
                return View();
            }
            else
            {
                CarOwnerHelper userHelp = new CarOwnerHelper();

                HttpCookie UserCookie = new HttpCookie("UserInfo");

                UserCookie["userId"] = count.userId.ToString();
                UserCookie.Expires = DateTime.Now.AddDays(30);
                Response.Cookies.Add(UserCookie);

                FormsAuthentication.SetAuthCookie(user.email, false);
                if (count.roleId == 1)
                {

                    return RedirectToAction("Index", "Customer");
                }
                else if (count.roleId == 2)
                {
                    var cookieHelper = new WorkShopHelper();
                    HttpCookie workShopCookie = cookieHelper.SetCookieForWorkshop(count);
                    workShopCookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(workShopCookie);
                    return RedirectToAction("Index", "VeheliceWorkShop");
                }
                else if (count.roleId == 3)
                {
                    var cookieHelper = new AdminHelper();
                    HttpCookie AdminCookie = cookieHelper.SetCookieForAdmin(count);
                    AdminCookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(AdminCookie);
                    return RedirectToAction("Index", "Admin");
                }

                return RedirectToAction("Index", "Guest");
            }

        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Guest");
        }

    }
}