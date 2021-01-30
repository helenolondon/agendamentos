using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agendamentos.Controllers.Apis.Requests
{
    public class SalvarAgendamentoItemRequest
    {
        public int CodAgendamentoItem { get; set; }
        public int CodServico { get; set; }
        public string HoraInicio { get; set; }
        public string HoraTermino { get; set; }
    }
}
