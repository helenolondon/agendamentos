using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agendamentos.Controllers.Apis.Requests
{
    public class SalvarAgendamentoRequest
    {
        public int? CodAgendamento { get; set; }
        public int? CodProcedimento { get; set; }
        public string HoraInicio { get; set; }
        public string HoraTermino { get; set; }
    }
}
