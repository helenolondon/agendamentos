using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agendamentos.Controllers.Apis.Requests
{
    /// <summary>
    /// Representa o request da api de salvar agendamentos
    /// </summary>
    public class SalvarAgendamentoRequest
    {
        public int CodAgendamento { get; set; }
        public DateTime Data { get; set; }
        public int CodStatus { get; set; }
        public int CodCliente { get; set; }
        public List<SalvarAgendamentoItemRequest> Itens { get; set; }
    }
}
