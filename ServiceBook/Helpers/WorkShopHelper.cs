using ServiceBook.DAL;
using ServiceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceBook.Helpers
{
    public class WorkShopHelper
    {

        public WorkShopHelper()
        {

        }
        public HttpCookie SetCookieForWorkshop(Users workShopOwner)
        {

            HttpCookie workshopCookie = new HttpCookie("WorkShop");
            var WorkShopData = GetWorkshopData(workShopOwner);

            workshopCookie["userId"] = workShopOwner.userId.ToString();
            workshopCookie["workshopId"] = WorkShopData.veheliceWorkshpoId.ToString();
            workshopCookie["DescritpionId"] = WorkShopData.descriptionId.ToString();
            workshopCookie["NIP"] = WorkShopData.NIP;


            return workshopCookie;
        }
        public VeheliceWorkshpos GetWorkshopData(Users owner)
        {

            using (var context = new ServiceBookContext())
            {
                var result = context.veheliceWorkshops.Select(x => x).Where(x => x.ownerId == owner.userId).FirstOrDefault();
                return result;
            }

        }



    }
}