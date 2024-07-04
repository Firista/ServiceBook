using ServiceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceBook.Helpers
{
    public class ServicesHelper
    {
        public Services service;
        public bool isChosen = false;

        public ServicesHelper(Services serv, bool isChosen)
        {

            service = serv;
            this.isChosen = isChosen;

        }



    }
}