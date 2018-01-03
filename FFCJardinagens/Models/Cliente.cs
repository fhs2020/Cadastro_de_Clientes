using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FFCJardinagens.Models
{
    public class Cliente
    {
        public int ID { get; set; }
        public String Empresa { get; set; }
        public String Nome { get; set; }
        public String Email { get; set; }
        public String Telefone { get; set; }
        public String Celular { get; set; }
        public String CNPJ { get; set; }
        public String Endereco { get; set; }
        public String Cidade { get; set; }
        public String UF { get; set; }
        public String CEP { get; set; }
    }
}