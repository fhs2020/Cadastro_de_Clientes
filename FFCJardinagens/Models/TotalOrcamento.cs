using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FFCJardinagens.Models
{
    public class TotalOrcamento
    {
        public int ID { get; set; }
        public int ClienteID { get; set; }
        public String ClienteNome { get; set; }
        public  List<Orcamento> OrcamentoLista { get; set; }
        public decimal? ValorTotal { get; set; }
        public DateTime? DataOrcamento { get; set; }
    }
}