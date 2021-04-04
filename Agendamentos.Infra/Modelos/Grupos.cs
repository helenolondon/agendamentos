using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Agendamentos.Infra.Modelos
{
    [Table("Grupos")]
    public class Grupo
    {
        [Key]
        public int Cod_Grupo { get; set; }
        public string Nome { get; set; }
    }
}
