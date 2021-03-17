using Agendamentos.Infra;
using Agendamentos.Servicos.DTO;
using Agendamentos.Servicos.Listas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos
{
    public class PessoaServico: ServicoBase, IPessoaServico
    {
        public PessoaServico(IRepositorios repositorio) : base(repositorio) { }
        public PessoasListaDTO ListarPessoas()
        {
            var temp = new Listas.PessoasListaDTO();

            var q = this.repositorio.PessoasRepositorio.Listar();

            if(q == null)
            {
                return null;
            }

            temp.LoadFromPessoaLista(q);

            return temp;
        }

        public PessoasListaDTO ListarProfissionais()
        {
            var temp = new Listas.PessoasListaDTO();

            var q = this.repositorio.PessoasRepositorio.ObterProfissionais();

            if (q == null)
            {
                return null;
            }

            temp.LoadFromPessoaLista(q);

            return temp;
        }
    }
}
