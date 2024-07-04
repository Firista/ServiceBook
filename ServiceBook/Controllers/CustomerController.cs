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
    [CustomAuthorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private ServiceBookContext db = new ServiceBookContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            var result = db.marks.Select(x => x).ToList();
            ViewBag.marki = result;
            var silniki = db.engines.Select(x => x).ToList();
            ViewBag.silniki = silniki;
            return View();
        }

        [AllowAnonymous]
        public ActionResult AddCar()
        {
            var result = db.marks.Select(x => x).ToList();
            ViewBag.marki = result;
            var silniki = db.engines.Select(x => x).ToList();
            ViewBag.silniki = silniki;
            return View();
        }

        [HttpPost]
        public ActionResult AddCar(Vehelices item1, int Marks, int Engines)
        {
            int engineId = Engines;
            int markId = Marks;

            var carOfUser = Request.Cookies["UserInfo"];
            int ownerId = int.Parse(carOfUser["userId"]);

            Vehelices newVehicle = new Vehelices(item1.veheliceName, item1.engineCapacity, item1.productionYear, engineId, markId);

            using (var context = new ServiceBookContext())
            {
                context.vehelices.Add(newVehicle);
                context.SaveChanges();
            }

            Customers_Vehelices customerVehicles = new Customers_Vehelices(ownerId, newVehicle.veheliceId);

            using (var context = new ServiceBookContext())
            {
                context.customer_vehelice.Add(customerVehicles);
                context.SaveChanges();
            }

            return RedirectToAction("ShowMyVehicles", "Customer");

        }

        public ActionResult ShowMyVehicles()
        {

            var carOfUser = Request.Cookies["UserInfo"];

            int ownerId = int.Parse(carOfUser["userId"]);

            List<int> ListOfCustomerVehelices = db.customer_vehelice.Where(x => x.userId == ownerId).Select(x => x.veheliceId).ToList();

            List<CarHelper> ListOfUserVehelices = new List<CarHelper>();

            for (int i = 0; i < ListOfCustomerVehelices.Count; i++)
            {
                int helperOfCustomerVehelices = ListOfCustomerVehelices[i];

                Vehelices helper = db.vehelices.Where(x => x.veheliceId == helperOfCustomerVehelices).FirstOrDefault();
                Marks markHelper = db.marks.Where(x => x.markId == helper.markId).FirstOrDefault();
                Engines engineHelper = db.engines.Where(x => x.engineId == helper.engineId).FirstOrDefault();

                CarHelper car = new CarHelper(helper.veheliceId, markHelper.markName, helper.veheliceName, engineHelper.engineName, helper.engineCapacity, helper.productionYear);

                ListOfUserVehelices.Add(car);
            }

            ViewBag.mojeSamochody = ListOfUserVehelices;

            return View();
        }
        public ActionResult Edit(int? id)
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
        public ActionResult Edit([Bind(Include = "veheliceId,markId,veheliceName,engineId,engineCapacity,productionYear")] Vehelices vehelices, int? id)
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

            return RedirectToAction("ShowMyVehicles", "Customer");
        }

        public ActionResult Delete(int? id)
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
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehelices vehelices = db.vehelices.Find(id);

            db.vehelices.Remove(vehelices);
            db.SaveChanges();

            return RedirectToAction("ShowMyVehicles", "Customer");
        }

        public ActionResult ShowAllWorkshops()
        {
            List<int> ListOfWorkshopsId = db.veheliceWorkshops.Select(x => x.veheliceWorkshpoId).ToList();

            List<VeheliceWorkshopHelper> ListOfWorkshopsWithTheirServices = new List<VeheliceWorkshopHelper>();

            for (int i = 0; i < ListOfWorkshopsId.Count; i++)
            {
                int helperOfWorkshop = ListOfWorkshopsId[i];

                VeheliceWorkshpos workshopHelper = db.veheliceWorkshops.Where(x => x.veheliceWorkshpoId == helperOfWorkshop).FirstOrDefault();
                Users userHelper = db.users.Where(x => x.userId == workshopHelper.ownerId).FirstOrDefault();

                var roleId = userHelper.roleId;

                if (roleId == 2)
                {
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

            }

            ViewBag.AllWorkshops = ListOfWorkshopsWithTheirServices;

            return View();
        }

        public ActionResult ShowMessegeForm(string WorkshopNip)
        {
            HttpCookie userCookie = Request.Cookies["UserInfo"];
            var context = new ServiceBookContext();
            var workshopOwnerId = context.veheliceWorkshops.Select(x => x).Where(x => x.NIP == WorkshopNip).FirstOrDefault();
            var ownerId = (int)workshopOwnerId.ownerId;
            var userId = Int32.Parse(userCookie["userId"]);
            List<int> usersId = new List<int>();
            usersId.Add(userId);
            usersId.Add(ownerId);
            ViewBag.Onwer_Id = ownerId;
            return View();
        }

        [HttpPost]
        public ActionResult FirstMessege(string messegeBody, int ownerId)
        {
            HttpCookie userCookie = Request.Cookies["UserInfo"];
            var userID = Int32.Parse(userCookie["userId"]);
            var context = new ServiceBookContext();
            var workshopOwnerId = context.veheliceWorkshops.Select(x => x).Where(x => x.ownerId == ownerId).FirstOrDefault();

            var newVisit = new Visits()
            {
                userId = userID,
                veheliceWorkshopId = workshopOwnerId.veheliceWorkshpoId,
                visitTerm = new DateTime(1990, 1, 1)
            };
            context.visits.Add(newVisit);
            context.SaveChanges();

            var visitId = context.visits.Select(x => x).OrderByDescending(x => x.visitId).Take(1).First();

            var conversation = new Conversations()
            {
                visitId = visitId.visitId,
                status = false
            };
            context.conversations.Add(conversation);
            context.SaveChanges();

            var conversationId = context.conversations.Select(x => x).OrderByDescending(x => x.visitId).Take(1).First();

            var newMessage = new Messeges()
            {
                userId = userID,
                messegeBody = messegeBody,
                messageDate = DateTime.Now,

                conversationId = conversationId.conversationId
            };

            context.messeges.Add(newMessage);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult MyVisits()
        {
            HttpCookie userCookie = Request.Cookies["UserInfo"];
            var userID = Int32.Parse(userCookie["userId"]);
            var context = new ServiceBookContext();
            List<CustomerVisitsContainer> conatiner = new List<CustomerVisitsContainer>();
            var myVisits = context.visits.Select(x => x).Where(x => x.userId == userID).OrderBy(x => x.veheliceWorkshopId).ToList();
            List<VeheliceWorkshpos> workshops = new List<VeheliceWorkshpos>();

            foreach (var visit in myVisits)
            {
                var workshop = context.veheliceWorkshops.Select(x => x).Where(x => x.veheliceWorkshpoId == visit.veheliceWorkshopId).FirstOrDefault();
                workshops.Add(workshop);
            }

            for (int i = 0; i < myVisits.Count; i++)
            {
                var VisitData = new CustomerVisitsContainer()
                {
                    workshop = workshops[i],
                    visitDate = myVisits[i].visitTerm.ToString("dd.MM.yyy"),
                    visitId = myVisits[i].visitId
                };
                conatiner.Add(VisitData);
            }


            ViewBag.Myvisits = conatiner;

            return View();
        }


        public ActionResult Messeges(int visitId)
        {
            var context = new ServiceBookContext();
            var conversation = context.conversations.Select(x => x).Where(x => x.visitId == visitId).FirstOrDefault();
            var messeges = context.messeges.Select(x => x).Where(x => x.conversationId == conversation.conversationId).OrderBy(x => x.messageDate).ToList();

            var MessegeFullData =
                (from mess in messeges
                 join user in context.users.Select(x => x) on mess.userId equals user.userId
                 orderby mess.messageDate
                 select new { Imie = user.name, Nazwisko = user.surname, Text = mess.messegeBody, DataWyslania = mess.messageDate }).ToList();

            List<MessegaHelper> AllMesseges = new List<MessegaHelper>();

            foreach (var result in MessegeFullData)
            {
                var msg = new MessegaHelper()
                {
                    name = result.Imie,
                    surname = result.Nazwisko,
                    text = result.Text,
                    date = result.DataWyslania.ToString("dd.MM.yyyy HH:mm")
                };

                AllMesseges.Add(msg);
            }
            var conversationss = context.conversations.Select(x => x).Where(x => x.conversationId == conversation.conversationId).FirstOrDefault();
            ViewBag.Messeges = AllMesseges;
            ViewBag.Conversationid = conversationss.conversationId;
            return View();
        }

        [HttpPost]
        public ActionResult SendMessage(string messegeBody, int ConverationId)
        {
            var cookie = Request.Cookies["UserInfo"];
            int userID = Int32.Parse(cookie["userId"]);
            var context = new ServiceBookContext();
            var time = DateTime.Now;
            var messege = new Messeges()
            {
                conversationId = ConverationId,
                messegeBody = messegeBody,
                userId = userID,
                messageDate = time

            };




            context.messeges.Add(messege);
            context.SaveChanges();
            var visit = context.conversations.Select(x => x).Where(x => x.conversationId == ConverationId).FirstOrDefault();

            return RedirectToAction("Messeges", new { visitId = visit.visitId });
        }



        public PartialViewResult Messeges1(string id)
        {
            var context = new ServiceBookContext();
            var visitId = Int32.Parse(id);
            var conversation = context.conversations.Select(x => x).Where(x => x.visitId == visitId).FirstOrDefault();
            var messeges = context.messeges.Select(x => x).Where(x => x.conversationId == conversation.conversationId).OrderBy(x => x.messageDate).ToList();

            var MessegeFullData =
                (from mess in messeges
                 join user in context.users.Select(x => x) on mess.userId equals user.userId
                 orderby mess.messageDate
                 select new { Imie = user.name, Nazwisko = user.surname, Text = mess.messegeBody, DataWyslania = mess.messageDate }).ToList();

            List<MessegaHelper> AllMesseges = new List<MessegaHelper>();

            foreach (var result in MessegeFullData)
            {
                var msg = new MessegaHelper()
                {
                    name = result.Imie,
                    surname = result.Nazwisko,
                    text = result.Text,
                    date = result.DataWyslania.ToString("dd.MM.yyyy HH:mm")
                };

                AllMesseges.Add(msg);
            }
            var conversationss = context.conversations.Select(x => x).Where(x => x.conversationId == conversation.conversationId).FirstOrDefault();
            ViewBag.Messeges = AllMesseges;
            ViewBag.Conversationid = conversationss.conversationId;
            return PartialView("_messegeboxCustomer");
        }


        public PartialViewResult ShowMessegeForm1(string id)
        {
            HttpCookie userCookie = Request.Cookies["UserInfo"];
            var context = new ServiceBookContext();
            var WorkshopNip = id;
            var workshopOwnerId = context.veheliceWorkshops.Select(x => x).Where(x => x.NIP == WorkshopNip).FirstOrDefault();
            var ownerId = (int)workshopOwnerId.ownerId;
            var userId = Int32.Parse(userCookie["userId"]);
            List<int> usersId = new List<int>();
            usersId.Add(userId);
            usersId.Add(ownerId);
            ViewBag.Onwer_Id = ownerId;
            return PartialView("_FirsMessege");
        }


    }
}