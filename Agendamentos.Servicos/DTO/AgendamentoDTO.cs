using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos.DTO
{
    /// <summary>
    /// Representação dos dados de um agendamento
    /// </summary>
    public class AgendamentoDTO
    {
        public int CodAgendamento { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Termino { get; set; }
        public int CodProcedimento { get; set; }
        public int CodProfissional { get; set; }
        public string NomeProfissional { get; set; }
    }
}
