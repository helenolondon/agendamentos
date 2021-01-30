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
        public DateTime HoraInicio { get; set; }
        public DateTime HoraTermino { get; set; }
    }
}
