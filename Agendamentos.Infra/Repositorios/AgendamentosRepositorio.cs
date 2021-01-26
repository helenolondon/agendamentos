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
        public List<AgendamentoProfissional> ListarAgendamentos()
        {
            return this.dbContext.AgendamentoProfissionais
                .Include(b => b.Procedimento)
                .Include(m => m.Procedimento.Pessoa)
                .Include(s => s.Procedimento.Servico)
                .ToList();
        }

        public bool RemoverAgendamento(int codAgendamento)
        {
            var temp = this.dbContext.AgendamentoProfissionais.Find(codAgendamento);

            if(temp == null)
            {
                return false;
            }

            this.dbContext.AgendamentoProfissionais.Remove(temp);
            this.dbContext.SaveChanges();

            return true;
        }
        public int SalvarAgendamento(AgendamentoProfissional agendamento)
        {
            var temp = this.dbContext.AgendamentoProfissionais.Find(agendamento.Cd_Agendamento);

            agendamento.Procedimento = this.dbContext.Procedimentos.Find(agendamento.Procedimento.Cd_Procedimento);

            if(temp != null)
            {
                temp.Cd_Procedimento = agendamento.Cd_Procedimento;
                temp.Data_Inicio = agendamento.Data_Inicio;
                temp.Data_Termino = agendamento.Data_Termino;
            }
            else
            {
                if(this.dbContext.AgendamentoProfissionais.Count() > 0)
                {
                    agendamento.Cd_Agendamento = this.dbContext.AgendamentoProfissionais.Max(id => id.Cd_Agendamento)+ 1;
                }
                else
                {
                    agendamento.Cd_Agendamento = 1;
                }


                this.dbContext.AgendamentoProfissionais.Add(agendamento);
            }

            this.dbContext.SaveChanges();

            return agendamento.Cd_Agendamento;
        }
    }
}
