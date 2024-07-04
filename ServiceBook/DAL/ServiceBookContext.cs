using ServiceBook.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Web;

namespace ServiceBook.DAL
{
    public class ServiceBookContext : DbContext
    {
       public  ServiceBookContext() :base("ServiceBook")
        {

    }
        public DbSet<Users> users { get; set; }
       public DbSet<Vehelices> vehelices { get; set; }
       public  DbSet<Marks> marks { get; set; }
       public  DbSet<Engines> engines { get; set; }
       public DbSet<Customers_Vehelices> customer_vehelice { get; set; }
       public DbSet<Visits> visits { get; set; }
       public DbSet<Conversations> conversations { get; set; }
       public DbSet<Messeges> messeges { get; set; }
       public DbSet<VeheliceWorkshpos> veheliceWorkshops { get; set; }
       public DbSet<Description> descriptions { get; set; }
       public DbSet<VeheliceWorkshops_Services> veheliceWorkshops_Services { get; set; }
       public DbSet<Services> services { get; set; }
       public DbSet<Role> roles { get; set; }

    }
}