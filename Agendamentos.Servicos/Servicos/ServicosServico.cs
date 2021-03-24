using Agendamentos.Infra;
using Agendamentos.Servicos.DTO;
using Agendamentos.Servicos.Listas;
using Agendamentos.ServicosNM;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos
{
    public class ServicosServico : ServicoBase, IServicosServico
    {
        public ServicosServico(IRepositorios repositorio): base(repositorio) { }
        
        public ServicosListaDTO Listar()
        {
            var q = this.repositorio.ServicosRepositorio.Listar();
            
            if(q != null && q.Count > 0)
            {
                var temp = new ServicosListaDTO();
                temp.LoadFromServicoLista(q);

                return temp;
            }

            return null;
        }
    }
}
