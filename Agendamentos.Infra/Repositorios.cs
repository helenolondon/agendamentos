using Agendamentos.Infra.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Infra.Repositorios
{
    public class Repositorios: IRepositorios
    {
        private readonly AgendamentosDbContext dbContext;            
        private IAgendamentosRepositorio agendamentosRepositorio;

        public Repositorios()
        {
            this.dbContext = new AgendamentosDbContext(this.RetConnectionString());
        }
        
        private string RetConnectionString()
        {
            
            return @"Provider=SQLNCLI11;Server=.\SQLEXPRESS;Database=Odontica;Uid=sa;Pwd=Gaia010265";
        }

        public IAgendamentosRepositorio AgendamentosRepositorio
        {
            get
            {
                if(this.agendamentosRepositorio == null)
                {
                    this.agendamentosRepositorio = new AgendamentosRepositorio(this.dbContext);
                }

                return this.agendamentosRepositorio;
            }
        }
    }
}
