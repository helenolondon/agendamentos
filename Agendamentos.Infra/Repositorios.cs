﻿using Agendamentos.Infra.EF;
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


        public Repositorios()
        {
            this.dbContext = new AgendamentosDbContext(this.RetConnectionString());
        }
        
        private string RetConnectionString()
        {            
            return @"Password=Gaia010265;Persist Security Info=True;User ID=sa;Initial Catalog=Odontica;Data Source=srv-praianorte2\sqlexpress";
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
    }
}
