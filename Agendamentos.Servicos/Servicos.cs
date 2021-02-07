using Agendamentos.Infra;
using Agendamentos.Infra.Repositorios;
using Microsoft.Extensions.Configuration;
using System;

namespace Agendamentos.Servicos
{
    public class Servicos
    {
        private readonly IRepositorios repositorios;

        private IAgendamentosServico agendamentosServico;
        private IPessoaServico pessoaServico;
        private IProcedimentoServico procedimentoServico;
        private IServicosServico servicosServico;

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
    }
}
