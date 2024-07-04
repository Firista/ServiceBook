using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceBook.Models
{
    public class Services
    {   [Key]
        public int serviceId { get; set; }
        public string serviceName { get; set; }
    }
}