using Agendamentos.Infra.Modelos;
using Agendamentos.Servicos.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos.Listas
{
    public class ProcedimentoListaDTO: List<ProcedimentoDTO>
    {
        public List<Procedimento> ToPessoaLista()
        {
            var temp = new List<Procedimento>();

            this.ForEach((e) =>
            {
                temp.Add(new Procedimento()
                {
                    Cd_DiaSemana = e.CodDiaSemana,
                    Cd_Pessoa = e.CodPessoa,
                    Cd_Procedimento = e.CodProcedimento,
                    Cd_Servico = e.CodServico,
                    Num_HoraFim = TimeSpan.Parse(e.Num_HoraFim),
                    Num_HoraInicio = TimeSpan.Parse(e.Num_HoraInicio),
                    Pessoa = null,
                    Servico = null
                });
            });

            return temp;
        }
        public void LoadrocedimentoLista(List<Procedimento> lista)
        {
            this.Clear();
            ProcedimentoDTO temp;

            lista.ForEach((e) =>
            {
                temp = new ProcedimentoDTO();
                temp.LoadFromProcedimento(e);

                this.Add(temp);
            });
        }
    }
}
