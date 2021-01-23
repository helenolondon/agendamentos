using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Agendamentos.Infra.Modelos
{
    [Table("ProcedimentosAgendamentos")]
    public class AgendamentoProfissional
    {
        [Key]
        [Required]
        public int Cd_Agendamento { get; set; }
        [Required]
        public int Cd_Procedimento { get; set; }
        [Required]
        public DateTime Data_Inicio { get; set; }
        [Required]
        public DateTime Data_Termino { get; set; }
        public AgendamentoProfissional ToAgendamentoProfissional()
        {
            var temp = new AgendamentoProfissional();

            temp.Cd_Agendamento = this.Cd_Agendamento;
            temp.Cd_Procedimento = this.Cd_Procedimento;
            temp.Data_Inicio = this.Data_Inicio;
            temp.Data_Termino = this.Data_Termino;

            return temp;
        }
    }
}
