using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos.Erros
{
    public class ServicosException: Exception
    {
        public ServicosException(string message): base(message)
        {

        }
    }
}
