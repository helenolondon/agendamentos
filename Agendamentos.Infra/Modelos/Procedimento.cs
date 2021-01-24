using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Agendamentos.Infra.Modelos
{
    [Table("ProfissionaisProcedimentos")]
    public class Procedimento
    {
        [Key]
        public int Cd_Procedimento { get; set; }
        public int Cd_Pessoa { get; set; }
        public int Cd_Servico { get; set; }
        public int Cd_DiaSemana { get; set; }
        public decimal Num_HoraInicio { get; set; }
        public decimal Num_HoraFim { get; set; }
    }
}
