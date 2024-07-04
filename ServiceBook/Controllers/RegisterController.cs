using ServiceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Web.Helpers;
using ServiceBook.DAL;
using ServiceBook.Helpers;

namespace ServiceBook.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        // GET: Register

        public ActionResult RegisterCustomer()
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
        public ActionResult RegisterCustomer(Users user)
        {

            PasswordHelper passHelper = new PasswordHelper();

            var newPassword = passHelper.GeneratePassword(user.password);
            Users newCustomer = new Users(user.name, user.surname, user.email, newPassword);


            if (!ModelState.IsValid)
            {
                return RedirectToAction("RegisterCustomer", "Register");
            }

            else
            {
                using (var context = new ServiceBookContext())
                {
                    context.users.Add(newCustomer);
                    context.SaveChanges();
                }
            }

            return RedirectToAction("Index", "Guest");
        }

        public ActionResult RegisterWorkShop()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterWorkShop(Users item1, VeheliceWorkshpos item2)
        {
            PasswordHelper passHelper = new PasswordHelper();
            int descriptionId;
            var newPassword = passHelper.GeneratePassword(item1.password);
            Users newOwner = new Users(item1.name, item1.surname, item1.email, newPassword);
            newOwner.roleId = 2;
            Users owner;

            using (var context = new ServiceBookContext())
            {
                context.users.Add(newOwner);
                context.SaveChanges();
                owner = context.users.Select(x => x).Where(x => x.email == item1.email).FirstOrDefault();
            }
            using (var context = new ServiceBookContext())
            {
                Description newDescription = new Description();
                context.descriptions.Add(newDescription);
                context.SaveChanges();
                var desc = context.descriptions.Select(x => x).OrderByDescending(x => x.descriptionId).Take(1).FirstOrDefault();
                descriptionId = desc.descriptionId;
            }


            int ownerId = owner.userId;
            VeheliceWorkshpos newWorkshop = new VeheliceWorkshpos(item2.workshopName, item2.NIP, ownerId);
            newWorkshop.descriptionId = descriptionId;
            newWorkshop.ownerId = owner.userId;

            using (var context = new ServiceBookContext())
            {
                context.veheliceWorkshops.Add(newWorkshop);
                context.SaveChanges();
            }

            return RedirectToAction("Index", "Guest");
        }

    }
}

