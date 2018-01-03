using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FFCJardinagens.Models
{
    public class Cart
    {
        public int ID { get; set; }
        public int ClienteID { get; set; }
        public int OrcamentoID { get; set; }
    }
}