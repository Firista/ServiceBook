using ServiceBook.DAL;
using ServiceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceBook.Controllers
{
    public class GuestController : Controller
    {

        [Authorize(Roles = "Customer")]
        public ActionResult Index()
        {
            ServiceBookContext context = new ServiceBookContext();

            var result = context.marks.Select(x => x).ToList();
            ViewBag.marki = result;
            Users user = new Users();
            Marks marki = new Marks();
            return View();
        }





    }
}