﻿using Agendamentos.Servicos.DTO;
using Agendamentos.Servicos.Listas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos
{
    public interface IAgendamentosServico
    {
        AgendamentosListaDTO Listar();
        int SalvarAgendamentoItem(AgendamentoItemDTO agendamento);
        bool RemoverAgendamento(int codAgendamento);
    }
}
