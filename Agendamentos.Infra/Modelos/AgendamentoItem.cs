using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Agendamentos.Infra.Modelos
{
    /// <summary>
    /// Representa um item de agendamento no banco de dados
    /// </summary>
    [Table("AgendamentosItens")]
    public class AgendamentoItem
    {
        [Key]
        public int Cd_AgendamentoItem { get; set; }
        [Required]
        public int Cd_Agendamento { get; set; }
        [Required]
        public int Cd_Servico { get; set; }
        public int Cd_Profissional { get; set; }
        [Required]
        public DateTime Dat_Inicio { get; set; }
        [Required]
        public DateTime Dat_Termino { get; set; }        
        [ForeignKey("Cd_Agendamento")]
        public Agendamento Agendamento { get; set; }
        [ForeignKey("Cd_Servico")]
        public Servico Servico { get; set; }
        [ForeignKey("Cd_Profissional")]
        public Pessoa Profissional { get; set; }
        public decimal? Num_ValorServico { get; set; }
        public decimal? Num_Comissao { get; set; }
        public string Pago { get; set; }
        [MaxLength(2000)]
        public string Txt_Observacao { get; set; }
    }
}
