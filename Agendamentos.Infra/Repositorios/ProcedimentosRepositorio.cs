using Agendamentos.Infra.EF;
using Agendamentos.Infra.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agendamentos.Infra.Repositorios
{
    public class ProcedimentosRepositorio: IProcedimentosRepositorio
    {
        private readonly AgendamentosDbContext dbContext;
        public ProcedimentosRepositorio(AgendamentosDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<Procedimento> ListarProcedimentos(int codProfiossional)
        {
            return this.dbContext.Procedimentos
                .Where(p => p.Cd_Pessoa == codProfiossional)
                .Include(t => t.Pessoa)
                .Include(s => s.Servico)
                .OrderBy(h => h.Num_HoraInicio)
                .ToList();
        }

        public List<Procedimento> ListarPorProfissionais(List<Pessoa> pessoas)
        {
            var q = this.dbContext.Procedimentos.Where(p => pessoas.Contains(p.Pessoa)).ToList();

            if (q != null && q.Count() > 0)
            {
                return q;
            }

            return null;
        }
        public List<Procedimento> ListarPorProfissionais(int diaSemana, TimeSpan inicio, TimeSpan termino, int codServico)
        {
            var procedimentos = this.dbContext.Procedimentos
                .Include(r => r.Pessoa)
                .Where(p => p.Cd_DiaSemana == diaSemana || p.Cd_DiaSemana == 0)
                .Where(p => p.Cd_Servico == codServico)
                .Where(p => p.Num_HoraInicio <= inicio && p.Num_HoraFim >= termino)
                .ToList();

            if(procedimentos == null || procedimentos.Count == 0)
            {
                return null;
            }
            
            return procedimentos;
        }
    }
}
