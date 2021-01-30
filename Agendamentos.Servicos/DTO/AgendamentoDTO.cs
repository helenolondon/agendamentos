using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos.DTO
{
    public class AgendamentoDTO
    {
        public int CodAgendamento { get; set; }
        public DateTime Data { get; set; }
        public int CodStatus { get; set; }
    }
}
