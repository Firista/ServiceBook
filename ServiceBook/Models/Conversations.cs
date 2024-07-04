using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ServiceBook.Models
{
    public class Conversations
    {   [Key]
        public int conversationId { get; set; }
        public int visitId { get; set; }
        [ForeignKey("visitId")]
        public Visits visit { get; set; }
        public bool status { get; set; }

    }
}