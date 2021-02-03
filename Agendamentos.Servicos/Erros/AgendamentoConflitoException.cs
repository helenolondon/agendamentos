using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos.Erros
{
    public class AgendamentoConflitoException : Exception
    {
        public AgendamentoConflitoException(string message) : base(message)
        {
        }
    }
}
