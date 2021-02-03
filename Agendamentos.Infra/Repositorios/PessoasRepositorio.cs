using Agendamentos.Infra.EF;
using Agendamentos.Infra.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agendamentos.Infra.Repositorios
{
    public class PessoasRepositorio: IPessoasRepositorio
    {
        private readonly AgendamentosDbContext dbContext;
        public PessoasRepositorio(AgendamentosDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<Pessoa> Listar()
        {
            return this.dbContext.Pessoas
                .OrderBy(c => c.Txt_Nome)
                .ToList();
        }
        public Pessoa Obter(int codPessoa)
        {
            return this.dbContext.Pessoas
                .Where(c => c.Cod_Pessoa == codPessoa)
                .FirstOrDefault();
        }
    }
}
