using Agendamentos.Infra.EF;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Infra.Repositorios
{
    public class Repositorios: IRepositorios
    {
        private readonly AgendamentosDbContext dbContext;
        private IAgendamentosRepositorio agendamentosRepositorio;
        private IPessoasRepositorio pessoasRepositorio;
        private IProcedimentosRepositorio procedimentosRepositorio;
        private IServicosRepositorio servicosRepositorio;
        private ICompromissosRepositorio compromissosRepositorio;
        private IAutenticaRepositorio autenticaRepositorio;

        public Repositorios(IConfiguration configuration)
        {
            this.dbContext = new AgendamentosDbContext(configuration.GetConnectionString("agenda-connection-str"));
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
        public IPessoasRepositorio PessoasRepositorio
        {
            get
            {
                if (this.pessoasRepositorio == null)
                {
                    this.pessoasRepositorio = new PessoasRepositorio(this.dbContext);
                }

                return this.pessoasRepositorio;
            }
        }
        public IProcedimentosRepositorio ProcedimentosRepositorio
        {
            get
            {
                if (this.procedimentosRepositorio == null)
                {
                    this.procedimentosRepositorio = new ProcedimentosRepositorio(this.dbContext);
                }

                return this.procedimentosRepositorio;
            }
        }
        public IServicosRepositorio ServicosRepositorio
        {
            get
            {
                if (this.servicosRepositorio == null)
                {
                    this.servicosRepositorio = new ServicosRepositorio(this.dbContext);
                }

                return this.servicosRepositorio;
            }
        }
        public ICompromissosRepositorio CompromissosRepositorio
        {
            get
            {
                if (this.compromissosRepositorio == null)
                {
                    this.compromissosRepositorio = new CompromissosRepositorio(this.dbContext);
                }

                return this.compromissosRepositorio;
            }
        }
        public IAutenticaRepositorio AutenticaRepositorio
        {
            get
            {
                if (this.autenticaRepositorio == null)
                {
                    this.autenticaRepositorio = new AutenticaRepositorio(this.dbContext);
                }

                return this.autenticaRepositorio;
            }
        }
    }
}
