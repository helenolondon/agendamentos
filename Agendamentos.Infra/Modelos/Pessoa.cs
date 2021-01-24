using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Agendamentos.Infra.Modelos
{
    public class Pessoa
    {
        [Key]
        public int Cod_Pessoa { get; set; }
        [Required]
        public string Txt_Nome { get; set; }
    }
}
