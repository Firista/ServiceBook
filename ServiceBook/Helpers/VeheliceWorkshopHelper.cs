using ServiceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceBook.Helpers
{
    public class VeheliceWorkshopHelper
    {
        public int veheliceWorkshpoId { get; set; }
        public string workshopName { get; set; }
        public string NIP { get; set; }
        public string userName { get; set; }
        public string userSurname { get; set; }
        public string openHour { get; set; }
        public string closeHour { get; set; }
        public List<Services> serviceNames { get; set; }

        public VeheliceWorkshopHelper(int veheliceWorkshpoId, string workshopName, string NIP, string userName, string userSurname, string openHour, string closeHour, List<Services> serviceNames)
        {
            this.veheliceWorkshpoId = veheliceWorkshpoId;
            this.workshopName = workshopName;
            this.NIP = NIP;
            this.userName = userName;
            this.userSurname = userSurname;
            this.openHour = openHour;
            this.closeHour = closeHour;
            this.serviceNames = serviceNames;
        }
    }
}