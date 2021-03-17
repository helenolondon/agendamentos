using Agendamentos.Infra.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Infra.Repositorios
{
    public interface IAgendamentosRepositorio
    {
        List<AgendamentoItem> ListarAgendamentos(DateTime dataInicial, DateTime dataFinal, int codProfissional);
        Agendamento ConsultarAgendamento(int codAgendamento);
        List<AgendamentoItem> ListarAgendamentosItens(int? codProfissional, DateTime? dataInicial, DateTime? dataFinal);
        List<AgendamentoItem> ListarAgendamentosItens(System.Linq.Expressions.Expression<Func<AgendamentoItem, bool>> filtro);
        int SalvarAgendamento(AgendamentoItem agendamento);
        int SalvarAgendamento(Agendamento agendamento);
        bool RemoverAgendamento(int codAgendamentoItem);
    }
}
