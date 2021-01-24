using Agendamentos.Infra.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos.DTO
{
    public class PessoaDTO
    {
        public int CodPessoa { get; set; }
        public string NomePessoa { get; set; }

        internal void LoadFromPessoa(Pessoa e)
        {
            this.CodPessoa = e.Cod_Pessoa;
            this.NomePessoa = e.Txt_Nome;
        }

        internal Pessoa ToPessoa()
        {
            var temp = new Pessoa();

            temp.Cod_Pessoa = this.CodPessoa;
            temp.Txt_Nome = this.NomePessoa;

            return temp;
        }
    }
}
