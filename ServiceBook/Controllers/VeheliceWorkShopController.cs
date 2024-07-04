using ServiceBook.DAL;
using ServiceBook.Helpers;
using ServiceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ServiceBook.Controllers
{
    [CustomAuthorize(Roles = "Workshop")]
    public class VeheliceWorkShopController : Controller
    {
        private HttpCookie GetCookie()
        {
            var cookie = Request.Cookies["WorkShop"];
            if (cookie != null)
            {
                var context = new ServiceBookContext();
                if (cookie["DescritpionId"] != null)
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
                if (cookie["DescritpionId"] != null)
                {
                    var test = cookie["DescritpionId"];
                    int descriptionId = Int32.Parse(cookie["DescritpionId"]);
                    var description = context.descriptions.Select(x => x).Where(x => x.descriptionId == descriptionId).FirstOrDefault();
                    ViewBag.Opis = description;
                    int workshopId = Int32.Parse(cookie["workshopId"]);
                    var visits = context.visits.Select(x => x).Where(x => x.veheliceWorkshopId == workshopId).ToList();

                    var query =
                        (from wizyty in visits
                         join klienci in context.users.Select(x => x) on wizyty.userId equals klienci.userId
                         where wizyty.userId == klienci.userId
                         orderby wizyty.visitTerm
                         select new { VisitDate = wizyty.visitTerm, Imie = klienci.name, Nazwisko = klienci.surname }).ToList();

                    var VisityViewsList = new List<WorkshopVisitView>();

                    foreach (var visit in query)
                    {
                        var visitToShow = new WorkshopVisitView()
                        {
                            vistDate = visit.VisitDate.ToString("dd/MM/yyyy"),
                            clientName = visit.Imie,
                            clientSurname = visit.Nazwisko
                        };
                        VisityViewsList.Add(visitToShow);
                    }

                    ViewBag.Wizyty = VisityViewsList;
                }

            }
            return View();
        }
        public ActionResult EditDescription()
        {
            var cookie = Request.Cookies["WorkShop"];
            var context = new ServiceBookContext();
            if (cookie != null)
            {
                var test = cookie["DescritpionId"];
                int descriptionId = Int32.Parse(cookie["DescritpionId"]);
                var description = context.descriptions.Select(x => x).Where(x => x.descriptionId == descriptionId).FirstOrDefault();
                ViewBag.Opis = description;
            }
            return View();
        }
        [HttpPost]
        public ActionResult EditDescription(string openHour, string closeHour, string description)
        {
            var cookie = Request.Cookies["WorkShop"];
            var context = new ServiceBookContext();
            if (cookie != null)
            {
                var test = cookie["DescritpionId"];
                int descriptionId = Int32.Parse(cookie["DescritpionId"]);
                var descSelected = context.descriptions.Select(x => x).Where(x => x.descriptionId == descriptionId).FirstOrDefault();
                descSelected.openHour = openHour;
                descSelected.closeHour = closeHour;
                descSelected.descriptionBody = description;
                context.SaveChanges();
            }

            return RedirectToAction("EditDescription", "VeheliceWorkShop");
        }

        public ActionResult EditServices()
        {
            List<ServicesHelper> services = new List<ServicesHelper>();
            var cookie = Request.Cookies["WorkShop"];
            var context = new ServiceBookContext();
            if (cookie != null)
            {
                int wokrshopId = Int32.Parse(cookie["workshopId"]);
                var AllServices = context.services.Select(x => x).ToList();
                var workShopServices = context.veheliceWorkshops_Services.Select(x => x).Where(x => x.veheliceWorkshopId == wokrshopId).ToList();
                var listOwnedServices = new List<Services>();

                var workShopServices2 = context.veheliceWorkshops_Services.Select(x => x).Where(x => x.veheliceWorkshopId == wokrshopId).ToList();
                int workshopId = Int32.Parse(cookie["workshopId"]);

                var test =
                    (from serv in context.services
                     join serv_Work in context.veheliceWorkshops_Services on serv.serviceId equals serv_Work.serviceId
                     where serv_Work.veheliceWorkshopId == workshopId
                     select new { Id = serv.serviceId, Name = serv.serviceName }).ToList();

                List<Services> owned = new List<Services>();

                foreach (var ss in test)
                {
                    var NewSerw = new Services()
                    {
                        serviceId = ss.Id,
                        serviceName = ss.Name

                    };

                    owned.Add(NewSerw);
                }

                List<Services> restServices = new List<Services>();
                int licznik = 0;

                while (owned.Count != 0)
                {
                    int id = owned[licznik].serviceId;
                    for (int i = 0; i < AllServices.Count; i++)
                    {
                        int id2 = AllServices[i].serviceId;
                        if (id2.Equals(id) == true)
                        {

                            AllServices.RemoveAt(i);
                            break;
                        }

                    }

                    owned.RemoveAt(0);
                }

                restServices = AllServices;

                if (workShopServices.Count == 0)
                {
                    for (int i = 0; i < AllServices.Count; i++)
                        services.Add(new ServicesHelper(AllServices[i], false));
                    ViewBag.Services = services;
                    return View();
                }
                else
                {

                    foreach (var serv in test)
                    {
                        var servi = new Services()
                        {
                            serviceName = serv.Name,
                            serviceId = serv.Id
                        };
                        services.Add(new ServicesHelper(servi, true));


                    }

                    for (int i = 0; i < restServices.Count; i++)
                    {

                        var servi = new Services()
                        {
                            serviceName = restServices[i].serviceName,
                            serviceId = restServices[i].serviceId
                        };
                        services.Add(new ServicesHelper(servi, false));
                    }

                    ViewBag.Services = services;
                    return View();
                }

            }
            return View();
        }

        [HttpPost]
        public ActionResult AddService(int serviceId)
        {
            var context = new ServiceBookContext();
            var serviceToAdd = context.services.Select(x => x).Where(x => x.serviceId == serviceId).FirstOrDefault();
            var cookie = GetCookie();
            if (cookie != null)
            {
                var serv_workshop = new VeheliceWorkshops_Services()
                {
                    veheliceWorkshopId = Int32.Parse(cookie["workshopId"]),
                    serviceId = serviceId
                };
                context.veheliceWorkshops_Services.Add(serv_workshop);
                context.SaveChanges();
            }


            return RedirectToAction("EditServices", "VeheliceWorkShop");
        }

        [HttpPost]
        public ActionResult DeleteService(int serviceId)
        {
            var context = new ServiceBookContext();
            var cookie = GetCookie();
            int workshopId = Int32.Parse(cookie["workshopId"]);

            var serviceToDelete = context.veheliceWorkshops_Services.Select(x => x).Where(x => x.serviceId == serviceId && x.veheliceWorkshopId == workshopId).FirstOrDefault();
            if (cookie != null)
            {

                context.veheliceWorkshops_Services.Remove(serviceToDelete);
                context.SaveChanges();
            }

            return RedirectToAction("EditServices", "VeheliceWorkShop");
        }

        public ActionResult ManageVisits()
        {
            var context = new ServiceBookContext();
            var cookie = GetCookie();
            int workshopId = Int32.Parse(cookie["workshopId"]);

            var workshopVisits = context.visits.Select(x => x).Where(x => x.veheliceWorkshopId == workshopId).OrderBy(x => x.visitId).ToList();
            var conversationList = new List<Conversations>();
            foreach (var visit in workshopVisits)
            {
                var conversations = context.conversations.Select(x => x).Where(x => x.visitId == visit.visitId).FirstOrDefault();
                conversationList.Add(conversations);
            }

            List<ManageWorkshopVisit> visitsData = new List<ManageWorkshopVisit>();

            for (int i = 0; i < workshopVisits.Count; i++)
            {
                var vistitFullData = new ManageWorkshopVisit
                {
                    visit = workshopVisits[i],
                    visitConversation = conversationList[i]

                };

                foreach (var customer in workshopVisits)
                {
                    var user = context.users.Select(x => x).Where(x => x.userId == customer.userId).FirstOrDefault();
                    var userInfo = new Users
                    {
                        name = user.name,
                        surname = user.surname
                    };
                    vistitFullData.client = userInfo;
                }

                visitsData.Add(vistitFullData);
            }

            ViewBag.visitFullData = visitsData;
            ViewBag.Today = DateTime.Today.Date;

            return View();
        }

        [HttpPost]
        public ActionResult SetDateForVisit(DateTime termin, int visitId)
        {
            var context = new ServiceBookContext();
            var visit = context.visits.Select(x => x).Where(x => x.visitId == visitId).FirstOrDefault();

            visit.visitTerm = termin;
            context.SaveChanges();
            return RedirectToAction("ManageVisits", "VeheliceWorkShop");
        }


        public ActionResult WorkShopMesseges(int conversationId)
        {
            var context = new ServiceBookContext();

            var messeges = context.messeges.Select(x => x).Where(x => x.conversationId == conversationId).OrderBy(x => x.messageDate).ToList();

            var messegesFullData =
                (from mess in messeges
                 join users in context.users.Select(x => x) on mess.userId equals users.userId
                 where mess.userId == users.userId
                 orderby mess.messageDate
                 select new
                 { Imie = users.name, Nazwisko = users.surname, DataWiadomosci = mess.messageDate, Tresc = mess.messegeBody })
                .ToList();

            List<MessegaHelper> allMesseges = new List<MessegaHelper>();

            foreach (var userMesseges in messegesFullData)
            {
                var messg = new MessegaHelper()
                {
                    name = userMesseges.Imie,
                    surname = userMesseges.Nazwisko,
                    text = userMesseges.Tresc,
                    date = userMesseges.DataWiadomosci.ToString("dd.MM.yyyy HH:mm")

                };
                allMesseges.Add(messg);
            }

            ViewBag.Messeges = allMesseges;
            ViewBag.Conversationid = conversationId;
            return View();
        }
        [HttpPost]
        public ActionResult SendMessage(string messegeBody, int ConverationId)
        {
            var cookie = GetCookie();
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

            return RedirectToAction("ManageVisits", "VeheliceWorkShop");
        }


        public PartialViewResult WorkShopMesseges4(string id)
        {
            var context = new ServiceBookContext();
            var conversationId = Int32.Parse(id);
            var messeges = context.messeges.Select(x => x).Where(x => x.conversationId == conversationId).OrderBy(x => x.messageDate).ToList();

            var messegesFullData =
                (from mess in messeges
                 join users in context.users.Select(x => x) on mess.userId equals users.userId
                 where mess.userId == users.userId
                 orderby mess.messageDate
                 select new
                 { Imie = users.name, Nazwisko = users.surname, DataWiadomosci = mess.messageDate, Tresc = mess.messegeBody })
                .ToList();

            List<MessegaHelper> allMesseges = new List<MessegaHelper>();

            foreach (var userMesseges in messegesFullData)
            {
                var messg = new MessegaHelper()
                {
                    name = userMesseges.Imie,
                    surname = userMesseges.Nazwisko,
                    text = userMesseges.Tresc,
                    date = userMesseges.DataWiadomosci.ToString("dd.MM.yyyy HH:mm")

                };
                allMesseges.Add(messg);
            }

            ViewBag.Messeges = allMesseges;
            ViewBag.Conversationid = conversationId;
            return PartialView("_MessegeBox");
        }
    }
}