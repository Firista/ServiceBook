using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceBook.Models
{
    public class Marks
    {[Key]
        public int markId { get; set; }
        public string markName { get; set; }
    }
}