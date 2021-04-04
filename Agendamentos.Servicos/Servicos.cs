using Agendamentos.Infra;
using Agendamentos.Infra.Repositorios;
using Agendamentos.Servicos;
using Agendamentos.Servicos.Servicos;
using Microsoft.Extensions.Configuration;
using System;

namespace Agendamentos.ServicosNM
{
    public class Servicos
    {
        private readonly IRepositorios repositorios;

        private IAgendamentosServico agendamentosServico;
        private IPessoaServico pessoaServico;
        private IProcedimentoServico procedimentoServico;
        private IServicosServico servicosServico;
        private ICompromissosServico compromissosServico;
        private IAutenticaService autenticaService;

        public Servicos(IConfiguration configuration)
        {
            this.repositorios = new Repositorios(configuration);
        }

        public IAgendamentosServico AgendamentosServico 
        { 
            get 
            {
                if(this.agendamentosServico != null)
                {
                    return this.agendamentosServico;
                }

                this.agendamentosServico = new AgendamentosServico(this.repositorios);

                return this.agendamentosServico;
            } 
        }

        public IPessoaServico PessoaServico
        {
            get
            {
                if (this.pessoaServico != null)
                {
                    return this.pessoaServico;
                }

                this.pessoaServico = new PessoaServico(this.repositorios);

                return this.pessoaServico;
            }
        }
        public IProcedimentoServico ProcedimentoServico
        {
            get
            {
                if (this.procedimentoServico != null)
                {
                    return this.procedimentoServico;
                }

                this.procedimentoServico = new ProcedimentoServico(this.repositorios);

                return this.procedimentoServico;
            }
        }
        public IServicosServico ServicosServico
        {
            get
            {
                if (this.servicosServico != null)
                {
                    return this.servicosServico;
                }

                this.servicosServico = new ServicosServico(this.repositorios);

                return this.servicosServico;
            }
        }

        public ICompromissosServico CompromissosServico
        {
            get
            {
                if (this.compromissosServico != null)
                {
                    return this.compromissosServico;
                }

                this.compromissosServico = new CompromissosServico(this.repositorios);

                return this.compromissosServico;
            }
        }

        public IAutenticaService AutenticaService
        {
            get
            {
                if (this.autenticaService != null)
                {
                    return this.autenticaService;
                }

                this.autenticaService = new AutenticaService(this.repositorios);

                return this.autenticaService;
            }
        }
    }
}
