using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ServiceBook.Models
{
    public class Messeges
    {   [Key]
        public int messegeId { get; set; }
        public int conversationId { get; set; }
        [ForeignKey("conversationId")]
        public Conversations conversation { get; set; }
        [Required(ErrorMessage ="Wpisz treść wiadomości przed wysłaniem")]
        public string messegeBody { get; set; }
        public DateTime messageDate { get; set; }

        public int? userId { get; set; }
        [ForeignKey("userId")]
        public virtual Users user { get; set; }
      
    }
}