using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceBook.Models
{
    public class Description
    {
        [Key]
        public int descriptionId { get; set; }
        [StringLength(5)]
        public string openHour { get; set; }
        [StringLength(5)]
        public string closeHour { get; set; }
        public string descriptionBody { get; set; }
    }
}