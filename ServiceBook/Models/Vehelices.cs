using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceBook.Models
{
    public class Vehelices
    {  [Key]
       public int veheliceId { get; set; }
       public int markId { get; set; }
       [ForeignKey("markId")]
       public Marks mark { get; set; }
       public string veheliceName { get; set; }
       public int engineId { get; set; }
       [ForeignKey("engineId")]
       public Engines engine;
       public double engineCapacity { get; set; }
       public string productionYear { get; set; }

        public Vehelices()
        {

        }

        public Vehelices(string vehicleName, double engineCapacity, string productionYear, int engineId, int markId)
        {
            this.veheliceName = vehicleName;
            this.engineCapacity = engineCapacity;
            this.productionYear = productionYear;
            this.engineId = engineId;
            this.markId = markId;

        }
    }
}