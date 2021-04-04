using Agendamentos.Infra.EF;
using Agendamentos.Infra.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agendamentos.Infra.Repositorios
{
    public class AutenticaRepositorio: IAutenticaRepositorio
    {
        private readonly AgendamentosDbContext dbContext;
        public AutenticaRepositorio(AgendamentosDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TokensAcesso ObterToken(string auth)
        {
            return this.dbContext.TokensAcesso
                .Where(tk => tk.Usr_Token == auth)
                .SingleOrDefault();
        }

        public Usuario ObterUsuario(int codUsuario)
        {
            return this.dbContext.Usuarios
                .Include(usu => usu.Grupos)
                .ThenInclude(gru => gru.Grupo)
                .Where(usu => usu.Cod_Usuario == codUsuario)
                .FirstOrDefault();
        }
    }
}
