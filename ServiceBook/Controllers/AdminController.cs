using ServiceBook.DAL;
using ServiceBook.Helpers;
using ServiceBook.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ServiceBook.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ServiceBookContext db = new ServiceBookContext();

        private HttpCookie GetCookie()
        {
            var cookie = Request.Cookies["Admin"];
            if (cookie != null)
            {
                var context = new ServiceBookContext();
                if (cookie["userId"] != null)
                {

                    return cookie;
                }

                else return null;
            }

            else return null;
        }

        public ActionResult Index()
        {
            var cookie = GetCookie();
            if (cookie != null)
            {
                var context = new ServiceBookContext();
                if (cookie["userId"] != null)
                {
                    int userId = Int32.Parse(cookie["userId"]);
                    var adminInfo = context.users.Select(x => x).Where(x => x.userId == userId).FirstOrDefault();
                    ViewBag.adminInfo = adminInfo;
                }
            }
            return View();
        }

        // CRUD warsztatów //

        public ActionResult ShowAllWorkshops()
        {
            List<int> ListOfWorkshopsId = db.veheliceWorkshops.Select(x => x.veheliceWorkshpoId).ToList();

            List<VeheliceWorkshopHelper> ListOfWorkshopsWithTheirServices = new List<VeheliceWorkshopHelper>();

            for (int i = 0; i < ListOfWorkshopsId.Count; i++)
            {
                int helperOfWorkshop = ListOfWorkshopsId[i];

                VeheliceWorkshpos workshopHelper = db.veheliceWorkshops.Where(x => x.veheliceWorkshpoId == helperOfWorkshop).FirstOrDefault();
                Users userHelper = db.users.Where(x => x.userId == workshopHelper.ownerId).FirstOrDefault();
                Description descriptionHelper = db.descriptions.Where(x => x.descriptionId == workshopHelper.descriptionId).FirstOrDefault(); //poprawić z innej klasy

                List<VeheliceWorkshops_Services> workshopServicesHelper = db.veheliceWorkshops_Services.Where(x => x.veheliceWorkshopId == helperOfWorkshop).ToList();

                List<Services> serviceHelper = new List<Services>();

                for (int j = 0; j < workshopServicesHelper.Count; j++)
                {
                    int serviceId = workshopServicesHelper[j].serviceId;
                    Services service = db.services.Where(x => x.serviceId == serviceId).FirstOrDefault();

                    string serviceName = service.serviceName;

                    serviceHelper.Add(service);
                }

                VeheliceWorkshopHelper workshop = new VeheliceWorkshopHelper(workshopHelper.veheliceWorkshpoId, workshopHelper.workshopName, workshopHelper.NIP, userHelper.name, userHelper.surname, descriptionHelper.openHour, descriptionHelper.closeHour, serviceHelper);

                ListOfWorkshopsWithTheirServices.Add(workshop);
            }

            ViewBag.AllWorkshops = ListOfWorkshopsWithTheirServices;

            return View();
        }

        public ActionResult EditWorkshop(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VeheliceWorkshpos veheliceWorkshop = db.veheliceWorkshops.Find(id);

            if (veheliceWorkshop == null)
            {
                return HttpNotFound();
            }

            ViewBag.ownerId = new SelectList(db.users, "userId", "name", veheliceWorkshop.ownerId);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditWorkshop([Bind(Include = "veheliceWorkshpoId,workshopName,NIP,ownerID")] VeheliceWorkshpos veheliceWorkshop, int? id)
        {
            var veheliceWorkshopSelected = db.veheliceWorkshops.Where(x => x.veheliceWorkshpoId == id).FirstOrDefault();

            veheliceWorkshopSelected.workshopName = veheliceWorkshop.workshopName;
            veheliceWorkshopSelected.NIP = veheliceWorkshop.NIP;
            veheliceWorkshopSelected.ownerId = veheliceWorkshop.ownerId;

            db.Entry(veheliceWorkshopSelected).State = EntityState.Modified;
            db.SaveChanges();

            ViewBag.ownerId = new SelectList(db.users, "userId", "name", veheliceWorkshop.ownerId);

            return RedirectToAction("ShowAllWorkshops", "Admin");
        }

        public ActionResult BanWorkshop(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VeheliceWorkshpos veheliceWorkshpos = db.veheliceWorkshops.Find(id);
            if (veheliceWorkshpos == null)
            {
                return HttpNotFound();
            }

            Users veheliceWorkshopOwner = db.users.Find(veheliceWorkshpos.ownerId);
            veheliceWorkshopOwner.roleId = 4;

            db.Entry(veheliceWorkshopOwner).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ShowAllWorkshops", "Admin");
        }

        public ActionResult UnbanWorkshop(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VeheliceWorkshpos veheliceWorkshpos = db.veheliceWorkshops.Find(id);
            if (veheliceWorkshpos == null)
            {
                return HttpNotFound();
            }

            Users veheliceWorkshopOwner = db.users.Find(veheliceWorkshpos.ownerId);
            veheliceWorkshopOwner.roleId = 2;

            db.Entry(veheliceWorkshopOwner).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ShowAllWorkshops", "Admin");
        }

        // CRUD użytkowników //

        public ActionResult ShowAllUsers()
        {
            List<UsersHelper> allUsers = new List<UsersHelper>();

            List<int> usersId = db.users.Select(x => x.userId).ToList();

            for (int i = 0; i < usersId.Count; i++)
            {
                int userId = usersId[i];
                Users user = db.users.Where(x => x.userId == userId).Select(x => x).FirstOrDefault();
                Role role = db.roles.Where(x => x.roleId == user.roleId).Select(x => x).FirstOrDefault();

                UsersHelper userHelper = new UsersHelper(user.userId, user.name, user.surname, user.email, user.password, role.roleName);

                allUsers.Add(userHelper);
            }

            allUsers.OrderByDescending(x => x.roleName);

            ViewBag.AllUsers = allUsers;

            return View();
        }

        public ActionResult GiveRoleCustomer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Users user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            user.roleId = 1;

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ShowAllUsers", "Admin");
        }

        public ActionResult GiveRoleWorkshop(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Users user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            user.roleId = 2;

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ShowAllUsers", "Admin");
        }

        public ActionResult GiveRoleAdmin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Users user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            user.roleId = 3;

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ShowAllUsers", "Admin");
        }

        public ActionResult GiveRoleBanned(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Users user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            user.roleId = 4;

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ShowAllUsers", "Admin");
        }

        public ActionResult EditUserInfo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Users user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserInfo([Bind(Include = "userId,name,surname,email")] Users user, int? id)
        {
            var userSelected = db.users.Where(x => x.userId == id).FirstOrDefault();

            userSelected.name = user.name;
            userSelected.surname = user.surname;
            userSelected.email = user.email;

            db.Entry(userSelected).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ShowAllUsers", "Admin");
        }

        // CRUD samochodów //

        public ActionResult ShowAllVehelices()
        {
            List<CarHelper> allVehelices = new List<CarHelper>();

            List<int> vehelicesId = db.vehelices.Select(x => x.veheliceId).ToList();

            for (int i = 0; i < vehelicesId.Count; i++)
            {
                int veheliceId = vehelicesId[i];

                Vehelices vehelice = db.vehelices.Where(x => x.veheliceId == veheliceId).Select(x => x).FirstOrDefault();
                Marks mark = db.marks.Where(x => x.markId == vehelice.markId).FirstOrDefault();
                Engines engine = db.engines.Where(x => x.engineId == vehelice.engineId).FirstOrDefault();

                CarHelper carHelper = new CarHelper(vehelice.veheliceId, mark.markName, vehelice.veheliceName, engine.engineName, vehelice.engineCapacity, vehelice.productionYear);

                allVehelices.Add(carHelper);
            }

            ViewBag.AllVehelices = allVehelices;

            return View();
        }

        public ActionResult EditVehelice(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Vehelices vehelices = db.vehelices.Find(id);

            if (vehelices == null)
            {
                return HttpNotFound();
            }

            ViewBag.markId = new SelectList(db.marks, "markId", "markName", vehelices.markId);
            ViewBag.engineId = new SelectList(db.engines, "engineId", "engineName", vehelices.engineId);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditVehelice([Bind(Include = "veheliceId,markId,veheliceName,engineId,engineCapacity,productionYear")] Vehelices vehelices, int? id)
        {

            var veheliceSelected = db.vehelices.Where(x => x.veheliceId == id).FirstOrDefault();

            veheliceSelected.markId = vehelices.markId;
            veheliceSelected.veheliceName = vehelices.veheliceName;
            veheliceSelected.engineId = vehelices.engineId;
            veheliceSelected.engineCapacity = vehelices.engineCapacity;
            veheliceSelected.productionYear = vehelices.productionYear;

            db.Entry(veheliceSelected).State = EntityState.Modified;
            db.SaveChanges();

            ViewBag.markId = new SelectList(db.marks, "markId", "markName", vehelices.markId);
            ViewBag.engineId = new SelectList(db.engines, "engineId", "engineName", vehelices.engineId);

            return RedirectToAction("ShowAllVehelices", "Admin");
        }

        public ActionResult DeleteVehelice(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Vehelices vehelices = db.vehelices.Find(id);

            if (vehelices == null)
            {
                return HttpNotFound();
            }

            db.vehelices.Remove(vehelices);
            db.SaveChanges();

            return RedirectToAction("ShowAllVehelices", "Admin");
        }

        // CRUD marek //

        public ActionResult ShowAllMarks()
        {
            var marks = db.marks.Select(x => x).ToList();

            ViewBag.AllMarks = marks;

            return View();
        }

        public ActionResult AddMark()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMark([Bind(Include = "markId,markName")] Marks marks)
        {
            db.marks.Add(marks);
            db.SaveChanges();

            return RedirectToAction("ShowAllMarks", "Admin");
        }

        public ActionResult EditMark(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Marks marks = db.marks.Find(id);

            if (marks == null)
            {
                return HttpNotFound();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMark([Bind(Include = "markId,markName")] Marks marks, int? id)
        {
            var markSelected = db.marks.Find(id);

            markSelected.markName = marks.markName;

            db.Entry(markSelected).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ShowAllMarks", "Admin");
        }

        public ActionResult DeleteMark(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Marks marks = db.marks.Find(id);

            if (marks == null)
            {
                return HttpNotFound();
            }

            db.marks.Remove(marks);
            db.SaveChanges();

            return RedirectToAction("ShowAllMarks", "Admin");
        }

        // CRUD silników //

        public ActionResult ShowAllEngines()
        {
            var engines = db.engines.Select(x => x).ToList();

            ViewBag.AllEngines = engines;

            return View();
        }

        public ActionResult AddEngine()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEngine([Bind(Include = "engineId,engineName")] Engines engine)
        {
            db.engines.Add(engine);
            db.SaveChanges();

            return RedirectToAction("ShowAllEngines", "Admin");
        }

        public ActionResult EditEngine(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Engines engine = db.engines.Find(id);

            if (engine == null)
            {
                return HttpNotFound();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEngine([Bind(Include = "engineId,engineName")] Engines engine, int? id)
        {
            var engineSelected = db.engines.Find(id);

            engineSelected.engineName = engine.engineName;

            db.Entry(engineSelected).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ShowAllEngines", "Admin");
        }

        public ActionResult DeleteEngine(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Engines engine = db.engines.Find(id);

            if (engine == null)
            {
                return HttpNotFound();
            }

            db.engines.Remove(engine);
            db.SaveChanges();

            return RedirectToAction("ShowAllEngines", "Admin");
        }

        // CRUD servisów //

        public ActionResult ShowAllServices()
        {
            var services = db.services.Select(x => x).ToList();

            ViewBag.AllServices = services;

            return View();
        }

        public ActionResult AddService()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddService([Bind(Include = "serviceId,serviceName")] Services service)
        {
            db.services.Add(service);
            db.SaveChanges();

            return RedirectToAction("ShowAllServices", "Admin");
        }

        public ActionResult EditService(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Services service = db.services.Find(id);

            if (service == null)
            {
                return HttpNotFound();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditService([Bind(Include = "serviceId,serviceName")] Services service, int? id)
        {
            var serviceSelected = db.services.Find(id);

            serviceSelected.serviceName = service.serviceName;

            db.Entry(serviceSelected).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ShowAllServices", "Admin");
        }

        public ActionResult DeleteService(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Services service = db.services.Find(id);

            if (service == null)
            {
                return HttpNotFound();
            }

            db.services.Remove(service);
            db.SaveChanges();

            return RedirectToAction("ShowAllServices", "Admin");
        }
    }
}