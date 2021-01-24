﻿using Agendamentos.Infra.Modelos;
using Agendamentos.Servicos.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos.Listas
{
    public class AgendamentosListaDTO: List<AgendamentoDTO>
    {
        public List<AgendamentoProfissional> ToAgendamentoProfissionalLista()
        {
            var temp = new List<AgendamentoProfissional>();

            this.ForEach((e) =>
            {
                temp.Add(new AgendamentoProfissional()
                {
                    Cd_Agendamento = e.CodAgendamento,
                    Cd_Procedimento = e.CodProcedimento,
                    Data_Inicio = e.Inicio,
                    Data_Termino = e.Termino
                });
            });

            return temp;
        }
        public void LoadFromAgendamentoProfissionalLista(List<AgendamentoProfissional> lista)
        {
            this.Clear();

            lista.ForEach((e) =>
            {
                this.Add(new AgendamentoDTO()
                {
                    CodAgendamento = e.Cd_Agendamento,
                    CodProcedimento = e.Cd_Procedimento,
                    Inicio = e.Data_Inicio,
                    Termino = e.Data_Termino
                });
            });
        }
    }
}