using ServiceBook.DAL;
using ServiceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceBook.Helpers
{
    public class CarOwnerHelper
    {
        public HttpCookie SetCookieForVehelice(int userId, int veheliceId)
        {
            HttpCookie VeheliceOwnerCookie = new HttpCookie("VeheliceOwnerInfo");
            var VeheliceData = GetVeheliceData(userId);

            VeheliceOwnerCookie["userId"] = userId.ToString();
            VeheliceOwnerCookie["veheliceId"] = veheliceId.ToString();

            return VeheliceOwnerCookie;
        }

        public Vehelices GetVeheliceData(int veheliceId)
        {

            using (var context = new ServiceBookContext())
            {
                var result = context.vehelices.Select(x => x).Where(x => x.veheliceId == veheliceId).FirstOrDefault();
                return result;
            }

        }

    }
}