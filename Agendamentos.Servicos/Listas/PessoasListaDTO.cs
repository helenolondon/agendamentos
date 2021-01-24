using Agendamentos.Infra.Modelos;
using Agendamentos.Servicos.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos.Listas
{
    public class PessoasListaDTO: List<PessoaDTO>
    {
        public List<Pessoa> ToPessoaLista()
        {
            var temp = new List<Pessoa>();

            this.ForEach((e) =>
            {
                temp.Add(new Pessoa()
                {
                    Cod_Pessoa = e.CodPessoa,
                    Txt_Nome = e.NomePessoa
                });
            });

            return temp;
        }
        public void LoadFromPessoaLista(List<Pessoa> lista)
        {
            this.Clear();
            PessoaDTO temp;

            lista.ForEach((e) =>
            {
                temp = new PessoaDTO();
                temp.LoadFromPessoa(e);

                this.Add(temp);
            });
        }
    }
}
