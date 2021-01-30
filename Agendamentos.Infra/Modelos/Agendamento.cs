using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Agendamentos.Infra.Modelos
{
    /// <summary>
    /// Representa um agendamento no banco de dados
    /// </summary>
    public class Agendamento
    {
        [Key]
        public int Cd_Agendamento { get; set; }
        [Required]
        public int Cd_Cliente { get; set; }
        public DateTime Dat_Agendamento { get; set; }
        public int Cd_Status { get; set; }
        [ForeignKey("Cd_Cliente")]
        public Pessoa Cliente { get; set; }
        public List<AgendamentoItem> Itens { get; set; }
    }
}
