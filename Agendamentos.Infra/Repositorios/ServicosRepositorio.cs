using Agendamentos.Infra.EF;
using Agendamentos.Infra.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agendamentos.Infra.Repositorios
{
    public class ServicosRepositorio: IServicosRepositorio
    {
        private readonly AgendamentosDbContext dbContext;
        public ServicosRepositorio(AgendamentosDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Servico> Listar()
        {
            return this.dbContext.Servicos
                .OrderBy(s => s.Nome_Servico)
                .ToList();
        }
        public Servico ListarPorCodigo(int id)
        {
            return this.dbContext.Servicos
                .OrderBy(s => s.Id_Servico == id)
                .FirstOrDefault();
        }
    }
}
