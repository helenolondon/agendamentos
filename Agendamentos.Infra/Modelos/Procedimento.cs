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
        [Column("Cd_Pessoa")]
        public int Cod_Pessoa { get; set; }
        [ForeignKey("Cod_Pessoa")]
        public Pessoa Pessoa { get; set; }
        public int Cd_Servico { get; set; }
        public int Cd_DiaSemana { get; set; }
        public TimeSpan Num_HoraInicio { get; set; }
        public TimeSpan Num_HoraFim { get; set; }
    }
}
