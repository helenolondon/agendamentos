using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public DateTime Dat_Agendamento { get; set; }
        public int Cd_Status { get; set; }
    }
}
