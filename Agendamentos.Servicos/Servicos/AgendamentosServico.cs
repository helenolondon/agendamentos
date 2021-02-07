using Agendamentos.Infra;
using Agendamentos.Infra.EF;
using Agendamentos.Infra.Modelos;
using Agendamentos.Infra.Repositorios;
using Agendamentos.Servicos.DTO;
using Agendamentos.Servicos.Erros;
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
            agendamento.Itens.ForEach((ag) => 
            {
                this.ValidaAgendamento(ag);
            });
            
            return this.repositorio.AgendamentosRepositorio.SalvarAgendamento(agendamento.ToAgendamento());
        }
        public int SalvarAgendamentoItem(AgendamentoItemDTO agendamento)
        {
            return this.repositorio.AgendamentosRepositorio.SalvarAgendamento(agendamento.ToAgendamento());
        }

        private void ValidaAgendamento(AgendamentoItemDTO agendamentoItem)
        {
            var valido = true;
            List<AgendamentoItem> hist;

            if(agendamentoItem.CodAgendamentoItem == 0)
            {
                hist = this.repositorio.AgendamentosRepositorio.ListarAgendamentosItens(a => a.Dat_Inicio >= agendamentoItem.Inicio && a.Dat_Inicio <= agendamentoItem.Termino &&
                    a.Cd_Profissional == agendamentoItem.CodProfissional && agendamentoItem.CodCliente != a.Agendamento.Cd_Cliente);

                valido &= hist == null || hist.Count == 0;

                hist = this.repositorio.AgendamentosRepositorio.ListarAgendamentosItens(a => a.Dat_Termino >= agendamentoItem.Inicio && a.Dat_Termino <= agendamentoItem.Termino &&
                    a.Cd_Profissional == agendamentoItem.CodProfissional && agendamentoItem.CodCliente != a.Agendamento.Cd_Cliente);

                valido &= hist == null || hist.Count == 0;

                hist = this.repositorio.AgendamentosRepositorio.ListarAgendamentosItens(a => a.Dat_Inicio < agendamentoItem.Inicio && a.Dat_Termino > agendamentoItem.Termino &&
                    a.Cd_Profissional == agendamentoItem.CodProfissional && agendamentoItem.CodCliente != a.Agendamento.Cd_Cliente);

                valido &= hist == null || hist.Count == 0;               
            }
            else
            {
                hist = this.repositorio.AgendamentosRepositorio.ListarAgendamentosItens(a => a.Dat_Inicio >= agendamentoItem.Inicio && a.Dat_Inicio <= agendamentoItem.Termino &&
                    a.Cd_Profissional == agendamentoItem.CodProfissional && agendamentoItem.CodCliente != a.Agendamento.Cd_Cliente && a.Cd_AgendamentoItem != agendamentoItem.CodAgendamentoItem);

                valido &= hist == null || hist.Count == 0;

                hist = this.repositorio.AgendamentosRepositorio.ListarAgendamentosItens(a => a.Dat_Termino >= agendamentoItem.Inicio && a.Dat_Termino <= agendamentoItem.Termino &&
                    a.Cd_Profissional == agendamentoItem.CodProfissional && agendamentoItem.CodCliente != a.Agendamento.Cd_Cliente && a.Cd_AgendamentoItem != agendamentoItem.CodAgendamentoItem);

                valido &= hist == null || hist.Count == 0;

                hist = this.repositorio.AgendamentosRepositorio.ListarAgendamentosItens(a => a.Dat_Inicio < agendamentoItem.Inicio && a.Dat_Termino > agendamentoItem.Termino &&
                    a.Cd_Profissional == agendamentoItem.CodProfissional && agendamentoItem.CodCliente != a.Agendamento.Cd_Cliente && a.Cd_AgendamentoItem != agendamentoItem.CodAgendamentoItem);

                valido &= hist == null || hist.Count == 0;
            }

            if (!valido)
            {
                var nome = this.repositorio.PessoasRepositorio.Obter(agendamentoItem.CodProfissional)?.Txt_Nome ?? "profissional não identificado";
                nome = nome.Split(" ")[0];

                throw new AgendamentoConflitoException(nome + " não está disponível no horário " + agendamentoItem.horarioLabel);
            }
        }
    }
}
