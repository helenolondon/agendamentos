using Agendamentos.Infra.EF;
using Agendamentos.Infra.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agendamentos.Infra.Repositorios
{
    public class ProfissionaisRepositorio: IProfissionaisRepositorio
    {
        private readonly AgendamentosDbContext dbContext;
        public ProfissionaisRepositorio(AgendamentosDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<Pessoa> Listar()
        {
            return this.dbContext.Pessoas
                .OrderBy(c => c.Txt_Nome)
                .ToList();
        }
    }
}
