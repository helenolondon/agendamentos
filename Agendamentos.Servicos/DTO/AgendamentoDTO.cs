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

        public string start { get { return this.Inicio.ToString("yyyy-MM-dd HH':'mm':'ss"); } }
        public string end { get { return this.Termino.ToString("yyyy-MM-dd HH':'mm':'ss"); } }
        public string id { get { return this.CodAgendamento.ToString(); } }
        public string title { get { return "Serviço: " + this.CodAgendamento.ToString(); } }
    }
}
