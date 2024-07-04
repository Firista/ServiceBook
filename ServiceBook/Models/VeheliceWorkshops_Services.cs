using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ServiceBook.Models
{
    public class VeheliceWorkshops_Services
    {   [Key]
        public int id { get; set; }
        public int veheliceWorkshopId { get; set; }
        [ForeignKey("veheliceWorkshopId")]
        public VeheliceWorkshpos veheliceWorkshpos { get; set; }
        public int serviceId { get; set; }
        [ForeignKey("serviceId")]
        public Services service { get; set; }
    }
}