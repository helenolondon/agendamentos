using Agendamentos.Infra;
using Agendamentos.Servicos.Listas;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public PessoasListaDTO ListarProfissionaisParaAgendamento(int diaSemana, TimeSpan inicio, TimeSpan termino, int codServico)
        {
            var procedimentos = this.repositorio.ProcedimentosRepositorio.ListarPorProfissionais(diaSemana, inicio, termino, codServico);

            if(procedimentos == null || procedimentos.Count == 0)
            {
                return null;
            }

            var profissionais = procedimentos.Select(p => p.Pessoa).Distinct().ToList();
            var dtos = new PessoasListaDTO();

            dtos.LoadFromPessoaLista(profissionais);            
            
            return dtos;
        }

    }
}
