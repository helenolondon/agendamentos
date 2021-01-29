using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agendamentos.Controllers.Apis.Requests
{
    public class ListarProfissionaisDisponiveisParaAgendamento
    {
        public DateTime HoraInicio { get; set; }
        public DateTime HoraTernmino { get; set; }
        public int CodServico { get; set; }
    }
}
