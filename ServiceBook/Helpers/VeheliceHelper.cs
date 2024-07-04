using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceBook.Helpers
{
    public class VeheliceHelper
    {
        public int veheliceId { get; set; }
        public string markName { get; set; }
        public string veheliceName { get; set; }
        public string engineName { get; set; }
        public double engineCapacity { get; set; }
        public string productionYear { get; set; }
        public string userName { get; set; }
        public string userSurname { get; set; }

        public VeheliceHelper(int veheliceId, string markName, string vehicleName, string engineName, double engineCapacity, string productionYear, string userName, string userSurname)
        {
            this.veheliceId = veheliceId;
            this.markName = markName;
            this.veheliceName = vehicleName;
            this.engineName = engineName;
            this.engineCapacity = engineCapacity;
            this.productionYear = productionYear;
            this.userName = userName;
            this.userSurname = userSurname;
        }
    }
}