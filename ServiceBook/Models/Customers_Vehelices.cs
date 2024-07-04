using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ServiceBook.Models
{
    public class Customers_Vehelices
    {   [Key]
        public int id { get; set; }
        public int userId { get; set; }
        [ForeignKey("userId")]
        public virtual Users user { get; set; }
        public int veheliceId { get; set; }
        [ForeignKey("veheliceId")]
        public Vehelices vehelice { get; set; }

        public Customers_Vehelices(int userId, int vehicleId)
        {
            this.userId = userId;
            this.veheliceId = vehicleId;
        }
    }
}