using ServiceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceBook.Helpers
{
    public class ManageWorkshopVisit
    {
        public Users client;
        public Visits visit { get; set; }
        public Conversations visitConversation { get; set; }


        public ManageWorkshopVisit()
        {

        }

    }
}