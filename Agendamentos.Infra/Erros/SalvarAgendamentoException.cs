using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Infra.Erros
{
    public class SalvarAgendamentoException: Exception
    {
        public SalvarAgendamentoException(string message) : base(message) { }
    }
}
