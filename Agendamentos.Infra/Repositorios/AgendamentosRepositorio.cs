using Agendamentos.Infra.EF;
using Agendamentos.Infra.Erros;
using Agendamentos.Infra.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agendamentos.Infra.Repositorios
{
    public class AgendamentosRepositorio : IAgendamentosRepositorio
    {
        private readonly AgendamentosDbContext dbContext;
        public AgendamentosRepositorio(AgendamentosDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<AgendamentoItem> ListarAgendamentos()
        {
            return this.dbContext.AgendamentoItens
                .Include(b => b.Servico)
                .Include(a => a.Agendamento)
                .Include(p => p.Profissional)
                .Include(c => c.Agendamento.Cliente)
                .ToList();
        }
        public List<AgendamentoItem> ListarAgendamentosItens(System.Linq.Expressions.Expression<Func<AgendamentoItem, bool>> filtro)
        {
            return this.dbContext.AgendamentoItens
                .Where(filtro)
                .Include(b => b.Servico)
                .Include(a => a.Agendamento)
                .Include(p => p.Profissional)
                .Include(c => c.Agendamento.Cliente)
                .Include(m => m.Servico)
                .ToList();
        }
        public List<AgendamentoItem> ListarAgendamentosItens(int? codProfissional, DateTime? dataInicial, DateTime? dataFinal)
        {
            return this.dbContext.AgendamentoItens
                .Where(p => p.Cd_Profissional == codProfissional || !codProfissional.HasValue)
                .Where(d => d.Dat_Inicio >= dataInicial || !dataInicial.HasValue)
                .Where(d => d.Dat_Termino <= dataFinal || !dataFinal.HasValue)
                .Include(b => b.Servico)
                .Include(a => a.Agendamento)
                .Include(p => p.Profissional)
                .Include(c => c.Agendamento.Cliente)
                .Include(m => m.Servico)
                .ToList();
        }

        public Agendamento ConsultarAgendamento(int codAgendamento)
        {
            var q = this.dbContext.Agendamentos
                .Include(a => a.Cliente)
                .Include(a => a.Itens)
                .ThenInclude(p => p.Servico)
                .Include(p => p.Itens)
                .ThenInclude(p => p.Servico)
                .Include(p => p.Itens)
                .ThenInclude(p => p.Profissional)
                .Where(a => a.Cd_Agendamento == codAgendamento)
                .FirstOrDefault();

            if (q != null)
            {
                return q;
            }

            return null;
        }

        public bool RemoverAgendamento(int codAgendamentoItem)
        {
            var temp = this.dbContext.AgendamentoItens.Find(codAgendamentoItem);

            if(temp == null)
            {
                return false;
            }

            this.dbContext.AgendamentoItens.Remove(temp);
            this.dbContext.SaveChanges();

            return true;
        }

        private bool ValidaAgendamento(Agendamento agendamento)
        {
            if(agendamento == null)
            {
                throw new SalvarAgendamentoException("Erro: Agendamento nulo");
            }

            if(agendamento.Dat_Agendamento == null)
            {
                throw new SalvarAgendamentoException("Erro: Agendamento sem data");
            }

            if (agendamento.Itens.Count() == 0)
            {
                throw new SalvarAgendamentoException("Erro: Agendamento sem itens");
            }
            
            if (agendamento.Cd_Cliente == 0)
            {
                throw new SalvarAgendamentoException("Erro: Agendamento sem cliente");
            }

            return true;
        }

        public int SalvarAgendamento(Agendamento agendamento)
        {
            if (!ValidaAgendamento(agendamento))
            {
                return 0;
            }

            var model = this.dbContext.Agendamentos.Where(a => a.Cd_Agendamento == agendamento.Cd_Agendamento).Include(i => i.Itens).FirstOrDefault();

            if(agendamento.Cd_Agendamento == 0)
            {
                var codAgendamento = this.dbContext.Agendamentos.Count()== 0 ? 1 : this.dbContext.Agendamentos.Max(a => a.Cd_Agendamento) + 1;
                var CodAgendamentoItem = this.dbContext.AgendamentoItens.Count() == 0 ? 1 : this.dbContext.AgendamentoItens.Max(a => a.Cd_AgendamentoItem) + 1;

                for (var i = 0; i < agendamento.Itens.Count; i++)
                {
                    agendamento.Itens[i].Cd_AgendamentoItem = CodAgendamentoItem++;
                    agendamento.Itens[i].Cd_Agendamento = codAgendamento;
                }

                agendamento.Cd_Agendamento = codAgendamento;

                this.dbContext.Agendamentos.Add(agendamento);
                
                model = agendamento;
            }
            else
            {
                model.Cd_Status = agendamento.Cd_Status;
                model.Dat_Agendamento = agendamento.Dat_Agendamento;

                model.Itens
                    .RemoveAll((i) =>
                    {
                        return agendamento.Itens.Where(a => a.Cd_AgendamentoItem == i.Cd_AgendamentoItem).FirstOrDefault() == null;
                    });

                agendamento.Itens.ForEach((e) =>
                {
                    if (e.Cd_AgendamentoItem == 0)
                    {
                        var novo = new AgendamentoItem()
                        {
                            Cd_Agendamento = agendamento.Cd_Agendamento,
                            Cd_Servico = e.Cd_Servico,
                            Dat_Inicio = e.Dat_Inicio,
                            Dat_Termino = e.Dat_Termino,
                            Cd_Profissional = e.Cd_Profissional,
                            Num_ValorServico = e.Num_ValorServico
                    };
                        
                        novo.Cd_AgendamentoItem = this.dbContext.AgendamentoItens.Count() == 0 ? 1 : this.dbContext.AgendamentoItens.Max(a => a.Cd_AgendamentoItem) + 1;

                        dbContext.AgendamentoItens.Add(novo);
                    }
                    else
                    {
                        var item = model.Itens.Where(i => i.Cd_AgendamentoItem == e.Cd_AgendamentoItem).FirstOrDefault();

                        if (item == null)
                        {
                            dbContext.AgendamentoItens.Add(e);
                        }
                        else
                        {
                            item.Cd_Servico = e.Cd_Servico;
                            item.Dat_Inicio = e.Dat_Inicio;
                            item.Dat_Termino = e.Dat_Termino;
                            item.Cd_Profissional = e.Cd_Profissional;
                            item.Num_ValorServico = e.Num_ValorServico;
                        };
                    }
                });
            }

            dbContext.SaveChanges();

            return model.Cd_Agendamento;
        }
        public int SalvarAgendamento(AgendamentoItem agendamento)
        {
            var temp = this.dbContext.AgendamentoItens.Find(agendamento.Cd_Agendamento);

            agendamento.Servico = this.dbContext.Servicos.Find(agendamento.Servico.Id_Servico);

            if(temp != null)
            {
                temp.Cd_Servico = agendamento.Cd_Servico;
                temp.Dat_Inicio = agendamento.Dat_Inicio;
                temp.Dat_Termino = agendamento.Dat_Termino;
                temp.Cd_Profissional = agendamento.Cd_Profissional;
            }
            else
            {
                if(this.dbContext.AgendamentoItens.Count() > 0)
                {
                    agendamento.Cd_Agendamento = this.dbContext.AgendamentoItens.Max(id => id.Cd_Agendamento)+ 1;
                }
                else
                {
                    agendamento.Cd_Agendamento = 1;
                }


                this.dbContext.AgendamentoItens.Add(agendamento);
            }

            this.dbContext.SaveChanges();

            return agendamento.Cd_Agendamento;
        }
    }
}
