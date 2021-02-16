using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos.Erros
{
    public class ReAbrirAgendamentoException : ServicosException
    {
        public ReAbrirAgendamentoException(string message) : base(message)
        {
        }
    }
}
