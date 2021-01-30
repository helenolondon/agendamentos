using Agendamentos.Infra.Modelos;
using Agendamentos.Servicos.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos.Listas
{
    public class AgendamentosListaDTO: List<AgendamentoItemDTO>
    {
        public List<AgendamentoItem> ToAgendamentoProfissionalLista()
        {
            var temp = new List<AgendamentoItem>();

            this.ForEach((e) =>
            {
                temp.Add(new AgendamentoItem()
                {
                    Cd_Agendamento = e.CodAgendamento,
                    Cd_Servico = e.CodServico,
                    Dat_Inicio = e.Inicio,
                    Dat_Termino = e.Termino
                });
            });

            return temp;
        }
        public void LoadFromAgendamentoProfissionalLista(List<AgendamentoItem> lista)
        {
            this.Clear();
            AgendamentoItemDTO temp;

            lista.ForEach((e) =>
            {
                temp = new AgendamentoItemDTO();
                temp.LoadFromAgendamento(e);

                this.Add(temp);
            });
        }
    }
}
