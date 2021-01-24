using Agendamentos.Infra.Repositorios;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Infra
{
    public interface IRepositorios
    {
        IAgendamentosRepositorio AgendamentosRepositorio { get; }
        IPessoasRepositorio PessoasRepositorio { get; }
        IProcedimentosRepositorio ProcedimentosRepositorio { get; }
    }
}
