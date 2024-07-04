using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ServiceBook.Models
{
    public class Visits
    {   [Key]
        public int visitId {get;set;}
        public int userId { get; set; }
        [ForeignKey("userId")]
        public Users user { get; set; }
        public int veheliceWorkshopId { get; set; }
        [ForeignKey("veheliceWorkshopId")]
        public VeheliceWorkshpos veheliceWorkshop { get; set; }
        public DateTime visitTerm { get; set; }

    }
}