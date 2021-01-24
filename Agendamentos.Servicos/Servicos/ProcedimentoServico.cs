using Agendamentos.Infra;
using Agendamentos.Servicos.Listas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos
{
    public class ProcedimentoServico: ServicoBase, IProcedimentoServico
    {
        public ProcedimentoServico(IRepositorios repositorio): base(repositorio) { }
        public ProcedimentoListaDTO ListaPorPessoa(int codPessoa)
        {
            var temp = new Listas.ProcedimentoListaDTO();

            var q = this.repositorio.ProcedimentosRepositorio.ListarProcedimentos(codPessoa);

            if (q == null)
            {
                return null;
            }

            temp.LoadrocedimentoLista(q);

            return temp;
        }
    }
}
