using Agendamentos.Servicos.DTO;
using Agendamentos.Servicos.Listas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos
{
    public interface IAgendamentosServico
    {
        AgendamentoItensListaDTO Listar();
        int SalvarAgendamentoItem(AgendamentoItemDTO agendamento);
        int SalvarAgendamento(AgendamentoDTO agendamento);
        bool RemoverAgendamento(int codAgendamento);
    }
}
