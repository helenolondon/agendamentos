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
    }
}
