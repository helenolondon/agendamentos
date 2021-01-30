﻿using System;
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
        public int Cd_Procedimento { get; set; }
        [Required]
        public int Cd_Cliente { get; set; }
        [Required]
        public DateTime Dat_Inicio { get; set; }
        [Required]
        public DateTime Dat_Termino { get; set; }

        [ForeignKey("Cd_Cliente")]
        public Pessoa Cliente { get; set; }

        [ForeignKey("Cd_Procedimento")]
        public Procedimento Procedimento { get; set; }

        [ForeignKey("Cd_Agendamento")]
        public Agendamento Agendamento { get; set; }
    }
}
