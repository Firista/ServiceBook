using ServiceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceBook.Helpers
{
    public class CustomerVisitsContainer
    {
        public VeheliceWorkshpos workshop { get; set; }
        public string visitDate { get; set; }
        public int visitId { get; set; }
        public CustomerVisitsContainer()
        {

        }
    }
}