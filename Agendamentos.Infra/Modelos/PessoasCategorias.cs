using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Agendamentos.Infra.Modelos
{
    [Table("PessoasCategorias")]
    public class PessoasCategorias
    {
        public int Cd_Categoria { get; set; }
        public int Cd_Pessoa { get; set; }
        [ForeignKey("Cd_Pessoa")]
        public Pessoa Pessoa { get; set; }
    }
}
