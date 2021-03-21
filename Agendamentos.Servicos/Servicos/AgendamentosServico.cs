using Agendamentos.Infra;
using Agendamentos.Infra.Modelos;
using Agendamentos.Servicos.DTO;
using Agendamentos.Servicos.Erros;
using Agendamentos.Servicos.Listas;
using System;
using System.Collections.Generic;

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

        public AgendamentoItensListaDTO Listar(DateTime dataInicial, DateTime dataFinal, int codProfissional)
        {
            var temp = this.repositorio.AgendamentosRepositorio.ListarAgendamentos(dataInicial, dataFinal, codProfissional);
            
            if(temp == null)
            {
                return null;
            }

            var lista = new AgendamentoItensListaDTO();

            lista.LoadFromAgendamentoLista(temp);

            return lista;
        }

        public AgendamentosConfiguracoesDTO ObterConfiguracoes()
        {
            var conf = this.repositorio.AgendamentosRepositorio.ObterConfiguracoes();

            if(conf != null)
            {
                var dto = new AgendamentosConfiguracoesDTO();
                dto.LoadFromConfiguracoes(conf);

                return dto;
            }

            return null;
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

                // Realizado, atualiza preço na data do fechamento
                if(agendamento.CodStatus == 3)
                {
                    var servico = this.repositorio.ServicosRepositorio.ListarPorCodigo(ag.CodServico);
                    
                    if(servico != null)
                    {
                        ag.ValorServico = servico.Valor;
                    }

                    var comissaoNoDia = this.repositorio.ProcedimentosRepositorio
                    .ObterComissao(ag.Inicio, ag.Inicio.TimeOfDay, ag.Termino.TimeOfDay, ag.CodServico);

                    ag.Comissao = comissaoNoDia;
                }
            });
            
            return this.repositorio.AgendamentosRepositorio.SalvarAgendamento(agendamento.ToAgendamento());
        }
        public int SalvarAgendamentoItem(AgendamentoItemDTO agendamento)
        {
            return this.repositorio.AgendamentosRepositorio.SalvarAgendamento(agendamento.ToAgendamento());
        }

        private void ValidaConfigurações(AgendamentosConfiguracoesDTO conf)
        {
            if (!(TimeSpan.Parse(conf.FuncFinal) > TimeSpan.Parse(conf.FuncInicio)))
            {
                throw new ServicosException("Início do início do horário de funcionamento precisar ser menor do que o término");
            }
            
            if (!(TimeSpan.Parse(conf.AlmocFinal) > TimeSpan.Parse(conf.AlmocInicio)))
            {
                throw new ServicosException("Início do início do horário de almoço precisar ser menor do que o término");
            }

            if (conf.BloqAlmoco > 1 || conf.BloqAlmoco < 0)
            {
                throw new ServicosException("O Valor do campo de bloqueio de almoço é inválido");
            }

            if (conf.DispSegunda > 1 || conf.DispSegunda < 0)
            {
                throw new ServicosException("O Valor do campo de disponível segunda é inválido");
            }
            
            if (conf.DispTerca > 1 || conf.DispTerca < 0)
            {
                throw new ServicosException("O Valor do campo de disponível terça é inválido");
            }
            
            if (conf.DispQuarta> 1 || conf.DispQuarta < 0)
            {
                throw new ServicosException("O Valor do campo de disponível quarta é inválido");
            }
            
            if (conf.DispQuinta > 1 || conf.DispQuinta < 0)
            {
                throw new ServicosException("O Valor do campo de disponível quinta é inválido");
            }
            
            if (conf.DispSexta > 1 || conf.DispSexta < 0)
            {
                throw new ServicosException("O Valor do campo de disponível sexta é inválido");
            }
            
            if (conf.DispSabado > 1 || conf.DispSabado < 0)
            {
                throw new ServicosException("O Valor do campo de disponível sábado é inválido");
            }
            
            if (conf.DispDomingo > 1 || conf.DispDomingo < 0)
            {
                throw new ServicosException("O Valor do campo de disponível domingo é inválido");
            }
        }

        public bool SalvarConfiguracoes(AgendamentosConfiguracoesDTO conf)
        {
            if(conf == null)
            {
                throw new ServicosException("Configurações não podem estar vazias");
            }

            ValidaConfigurações(conf);

            return this.repositorio.AgendamentosRepositorio.SalvarConfiguracoes(conf.ToConfiguracoes()) > 0;
        }

        private void ValidaAgendamento(AgendamentoItemDTO agendamentoItem)
        {
            var valido = true;
            var conf = this.ObterConfiguracoes();
            List<AgendamentoItem> hist;

            var agOld = this.repositorio.AgendamentosRepositorio
                .ConsultarAgendamento(agendamentoItem.CodAgendamento);

            if(agOld != null && agOld.Cd_Status == 3)
            {
                throw new ServicosException("Não é permitido alterar um agendamento Realizado");
            }

            var conflitaAlmoco = false;
            var foraDeHorario = false;

            var inicioAlmoco = agendamentoItem.Inicio.Date + TimeSpan.Parse(conf.AlmocInicio);
            var fimAlmoco = agendamentoItem.Inicio.Date + TimeSpan.Parse(conf.AlmocFinal);
            
            var inicioExpediente = agendamentoItem.Inicio.Date + TimeSpan.Parse(conf.FuncInicio);
            var fimExpediente = agendamentoItem.Inicio.Date + TimeSpan.Parse(conf.FuncFinal);

            if (conf != null)
            {
                foraDeHorario = agendamentoItem.Inicio < inicioExpediente;
                foraDeHorario = foraDeHorario || agendamentoItem.Termino > fimExpediente;
                
                if(conf.BloqAlmoco == 1)
                {
                    conflitaAlmoco = inicioAlmoco >= agendamentoItem.Inicio && inicioAlmoco < agendamentoItem.Termino;
                    conflitaAlmoco = conflitaAlmoco || (fimAlmoco > agendamentoItem.Inicio && fimAlmoco <= agendamentoItem.Termino);
                    conflitaAlmoco = conflitaAlmoco || (inicioAlmoco < agendamentoItem.Inicio && fimAlmoco > agendamentoItem.Termino);
                }
            }

            if (foraDeHorario)
            {
                throw new ServicosException("Agendamento fora do horário permitido");
            }
            
            if (conflitaAlmoco)
            {
                throw new ServicosException("Não é permitido agendar durante o horário de almoço");
            }

            if (agendamentoItem.CodAgendamentoItem == 0)
            {
                hist = this.repositorio.AgendamentosRepositorio.ListarAgendamentosItens(a => a.Dat_Inicio >= agendamentoItem.Inicio && a.Dat_Inicio <= agendamentoItem.Termino &&
                    a.Cd_Profissional == agendamentoItem.CodProfissional && agendamentoItem.CodCliente != a.Agendamento.Cd_Cliente);

                valido &= hist == null || hist.Count == 0;

                hist = this.repositorio.AgendamentosRepositorio.ListarAgendamentosItens(a => a.Dat_Termino > agendamentoItem.Inicio && a.Dat_Termino <= agendamentoItem.Termino &&
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

                hist = this.repositorio.AgendamentosRepositorio.ListarAgendamentosItens(a => a.Dat_Termino > agendamentoItem.Inicio && a.Dat_Termino <= agendamentoItem.Termino &&
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

            if(agendamentoItem.Observacao.Length > 2000)
            {
                throw new ServicosException("Observação não pode ultrapassar 2000 caracteres");
            }
            
            if (agendamentoItem.CodStatus != 3)
            {
                if (agOld != null && agOld.Cd_Status == 3)
                {
                    throw new ReAbrirAgendamentoException("Agendamento já foi realizado e não pode ser reaberto");
                }
            }

            if(agendamentoItem.CodAgendamentoItem > 0 && agOld.Cd_Cliente != agendamentoItem.CodCliente)
            {
                throw new ServicosException("Não é permitido alterar o cliente de um agendamento existente");
            }
        }
    }
}
