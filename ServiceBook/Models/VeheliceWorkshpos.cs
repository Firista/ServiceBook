using ServiceBook.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ServiceBook.Models
{
    public class VeheliceWorkshpos
    {   [Key]
        public int veheliceWorkshpoId { get; set; }
        [Required(ErrorMessage = "Podaj nip do weryfikacji firmy" )]
        [StringLength(10)]
        public string NIP { get; set; }
        public int? ownerId { get; set; }
        [ForeignKey("ownerId")]
        public virtual Users user { get; set; }
        public string workshopName { get; set; }   
        public int? descriptionId { get; set; }
        [ForeignKey("descriptionId ")]
        public virtual Description description {get;set;}

        public VeheliceWorkshpos()
        {

        }

        public VeheliceWorkshpos(string workshopName, string NIP, int ownerID)
        {
            this.workshopName = workshopName;
            this.NIP = NIP;
          ///  this.ownerId = ownerID;
        }

        public Description GetDescription()
        {
            using (var context = new ServiceBookContext())
            {
                var descritpionSelect = context.descriptions.Select(x => x).Where(x => x.descriptionId == this.descriptionId).FirstOrDefault();
                return descritpionSelect;
            }
           
        }

    }
}