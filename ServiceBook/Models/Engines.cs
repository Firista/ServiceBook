using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceBook.Models
{
    public class Engines
    {   [Key]
        public int engineId { get; set; }
        public string engineName { get; set; }
    }
}