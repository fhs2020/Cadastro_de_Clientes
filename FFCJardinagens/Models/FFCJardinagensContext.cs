using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace FFCJardinagens.Models
{
    public class FFCJardinagensContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public FFCJardinagensContext() : base("name=FFCJardinagensContext")
        {
        }

        public System.Data.Entity.DbSet<FFCJardinagens.Models.Cliente> Clientes { get; set; }

        public System.Data.Entity.DbSet<FFCJardinagens.Models.Orcamento> Orcamentoes { get; set; }

        public System.Data.Entity.DbSet<FFCJardinagens.Models.Cart> Carts { get; set; }

        public System.Data.Entity.DbSet<FFCJardinagens.Models.TotalOrcamento> TotalOrcamentoes { get; set; }

    }
}
