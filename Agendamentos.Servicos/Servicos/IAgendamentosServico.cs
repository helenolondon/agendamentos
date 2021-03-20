using Agendamentos.Servicos.DTO;
using Agendamentos.Servicos.Listas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos
{
    public interface IAgendamentosServico
    {
        AgendamentosConfiguracoesDTO ObterConfiguracoes();
        bool SalvarConfiguracoes(AgendamentosConfiguracoesDTO conf);
        AgendamentoItensListaDTO Listar(DateTime dataInicial, DateTime dataFinal, int codProfissional);
        AgendamentoDTO Consultar(int codAgendamento);
        int SalvarAgendamentoItem(AgendamentoItemDTO agendamento);
        int SalvarAgendamento(AgendamentoDTO agendamento);
        bool RemoverAgendamento(int codAgendamento);
    }
}
