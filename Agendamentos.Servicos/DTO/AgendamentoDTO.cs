using Agendamentos.Infra.Modelos;
using Agendamentos.Servicos.Listas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos.DTO
{
    public class AgendamentoDTO
    {        
        public int CodAgendamento { get; set; }
        public DateTime Data { get; set; }
        public int CodStatus { get; set; }
        public int CodCliente { get; set; }

        public AgendamentoItensListaDTO Itens { get; set; }

        internal Agendamento ToAgendamento()
        {
            var ag = new Agendamento();

            ag.Cd_Agendamento = this.CodAgendamento;
            ag.Cd_Cliente = this.CodCliente;
            ag.Cd_Status = this.CodStatus;
            ag.Dat_Agendamento = this.Data;

            if(ag.Itens == null)
            {
                ag.Itens = new List<AgendamentoItem>();
            }

            this.Itens.ForEach((a) => 
            {
                ag.Itens.Add(a.ToAgendamento());
            });

            return ag;
        }
    }
}
