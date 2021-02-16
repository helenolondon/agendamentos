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
        [ForeignKey("Cd_Pessoa")]
        public Pessoa Pessoa { get; set; }
        public int Cd_Servico { get; set; }
        [ForeignKey("Cd_Servico")]
        public Servico Servico { get; set; }
        public int Cd_DiaSemana { get; set; }
        public TimeSpan Num_HoraInicio { get; set; }
        public TimeSpan Num_HoraFim { get; set; }        
    }
}
