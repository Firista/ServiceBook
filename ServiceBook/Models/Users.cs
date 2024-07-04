using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ServiceBook.Models
{
     public class Users
    {
        [Key]
        public int userId { get; set; }
       
        [Required(ErrorMessage = "Wpisz imie.")]
        [StringLength(30)]
        public string name { get; set; }

        [Required(ErrorMessage = "Wpisz nazwisko.")]
        [StringLength(30)]
        public string surname { get; set; }
        [Required(ErrorMessage = "Wpisz adres email, który będzie słuzył jako login.")]

        [EmailAddress]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        [Index("UQ_Email", 4, IsUnique = true)]
        public string email { get; set; }
        [Required(ErrorMessage = "Wpisz haslo.")]  
        public string password { get; set; }      
        public int? roleId { get; set; }
        [ForeignKey("roleId")]
        public virtual Role role {get; set;}

        public Users()
        {

        }

        public Users(string name, string surname, string email, string password)
        {
            this.name = name;
            this.surname = surname;
            this.email = email;
            this.password = password;
            this.roleId = 1;
        }
             
    }
}