using Agendamentos.Infra;
using Agendamentos.Infra.EF;
using Agendamentos.Infra.Repositorios;
using Agendamentos.Servicos.DTO;
using Agendamentos.Servicos.Listas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos
{
    public class AgendamentosServico : ServicoBase, IAgendamentosServico
    {
        public AgendamentosServico(IRepositorios repositorio) : base(repositorio) { }

        public AgendamentosListaDTO Listar()
        {
            var temp = this.repositorio.AgendamentosRepositorio.ListarAgendamentos();
            
            if(temp == null)
            {
                return null;
            }

            var lista = new AgendamentosListaDTO();

            lista.LoadFromAgendamentoProfissionalLista(temp);

            return lista;
        }
    }
}
