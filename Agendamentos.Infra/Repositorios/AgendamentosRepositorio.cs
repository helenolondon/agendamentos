using Agendamentos.Infra.EF;
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
                .Include(b => b.Procedimento)
                .Include(m => m.Procedimento.Pessoa)
                .Include(s => s.Procedimento.Servico)
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
        public int SalvarAgendamento(AgendamentoItem agendamento)
        {
            var temp = this.dbContext.AgendamentoItens.Find(agendamento.Cd_Agendamento);

            agendamento.Procedimento = this.dbContext.Procedimentos.Find(agendamento.Procedimento.Cd_Procedimento);

            if(temp != null)
            {
                temp.Cd_Procedimento = agendamento.Cd_Procedimento;
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
