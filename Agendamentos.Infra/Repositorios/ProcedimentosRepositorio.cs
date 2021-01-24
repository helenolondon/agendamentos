using Agendamentos.Infra.EF;
using Agendamentos.Infra.Modelos;
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
                .OrderBy(h => h.Num_HoraFim)
                .ToList();
        }
    }
}
