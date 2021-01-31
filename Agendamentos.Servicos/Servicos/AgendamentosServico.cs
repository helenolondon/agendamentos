﻿using Agendamentos.Infra;
using Agendamentos.Infra.EF;
using Agendamentos.Infra.Repositorios;
using Agendamentos.Servicos.DTO;
using Agendamentos.Servicos.Listas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agendamentos.Servicos
{
    public class AgendamentosServico : ServicoBase, IAgendamentosServico
    {
        public AgendamentosServico(IRepositorios repositorio) : base(repositorio) { }

        public AgendamentoDTO Consultar(int codAgendamento)
        {
            var q = this.repositorio.AgendamentosRepositorio.ConsultarAgendamento(codAgendamento);
            
            if(q != null) 
            {
                var novo = new AgendamentoDTO();
                novo.LoadFromAgendamento(q);

                return novo;
            }

            return null;
        }

        public AgendamentoItensListaDTO Listar()
        {
            var temp = this.repositorio.AgendamentosRepositorio.ListarAgendamentos();
            
            if(temp == null)
            {
                return null;
            }

            var lista = new AgendamentoItensListaDTO();

            lista.LoadFromAgendamentoLista(temp);

            return lista;
        }

        public bool RemoverAgendamento(int codAgendamentoItem)
        {
            return this.repositorio.AgendamentosRepositorio.RemoverAgendamento(codAgendamentoItem);
        }
        public int SalvarAgendamento(AgendamentoDTO agendamento)
        {
            return this.repositorio.AgendamentosRepositorio.SalvarAgendamento(agendamento.ToAgendamento());
        }
        public int SalvarAgendamentoItem(AgendamentoItemDTO agendamento)
        {
            return this.repositorio.AgendamentosRepositorio.SalvarAgendamento(agendamento.ToAgendamento());
        }
    }
}
