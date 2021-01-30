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
                .Include(c => c.Agendamento.Cliente)
                .Include(m => m.Servico)
                .ToList();
        }

        public bool RemoverAgendamento(int codAgendamento)
        {
            var temp = this.dbContext.AgendamentoItens.Find(codAgendamento);

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

            return true;
        }

        public int SalvarAgendamento(Agendamento agendamento)
        {
            if (!ValidaAgendamento(agendamento))
            {
                return 0;
            }

            var model = this.dbContext.Agendamentos.Where(a => a.Cd_Agendamento == agendamento.Cd_Agendamento).FirstOrDefault();

            if(agendamento.Cd_Agendamento == 0)
            {
                var codAgendamento = this.dbContext.Agendamentos.Max(a => a.Cd_Agendamento) + 1;
                
                agendamento.Cd_Agendamento = codAgendamento;

                this.dbContext.Agendamentos.Add(agendamento);
                
                model = agendamento;
            }
            else
            {
                model.Cd_Status = agendamento.Cd_Status;
                model.Dat_Agendamento = agendamento.Dat_Agendamento;
            }

            model.Itens.Clear();

            agendamento.Itens.ForEach((ag) =>
            {
                model.Itens.Add(ag);
            });

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
