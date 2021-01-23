using Agendamentos.Infra;
using Agendamentos.Infra.EF;
using Agendamentos.Infra.Repositorios;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos
{
    public class ServicoBase
    {
        protected readonly IRepositorios repositorio;
        public ServicoBase(IRepositorios repositorio)
        {
            this.repositorio = repositorio;
        }
    }
}
