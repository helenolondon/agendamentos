using Agendamentos.Infra.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Infra.Repositorios
{
    public interface IAgendamentosRepositorio
    {
        List<AgendamentoItem> ListarAgendamentos();
        int SalvarAgendamento(AgendamentoItem agendamento);
        int SalvarAgendamento(Agendamento agendamento);
        bool RemoverAgendamento(int codAgendamentoItem);
    }
}
