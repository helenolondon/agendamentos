using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Infra.Erros
{
    public class NaoEncontradoException: Exception
    {
        public NaoEncontradoException(string message) : base(message)  { }
    }
}
