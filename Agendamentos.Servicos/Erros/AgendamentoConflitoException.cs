﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos.Erros
{
    public class AgendamentoConflitoException : ServicosException
    {
        public AgendamentoConflitoException(string message) : base(message)
        {
        }
    }
}
