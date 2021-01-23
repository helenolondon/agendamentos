using Agendamentos.Infra.EF;
using Agendamentos.Infra.Modelos;
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
            return this.dbContext.AgendamentoProfissionais.ToList();
        }
    }
}
