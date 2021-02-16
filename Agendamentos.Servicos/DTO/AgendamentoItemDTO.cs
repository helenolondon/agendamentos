using Agendamentos.Infra.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos.DTO
{
    /// <summary>
    /// Representação dos dados de um agendamento
    /// </summary>
    public class AgendamentoItemDTO
    {
        public int CodAgendamento { get; set; }
        public string DataAgendamento { get; set; }
        public int CodAgendamentoItem { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Termino { get; set; }
        public int CodProfissional { get; set; }
        public string NomeProfissional { get; set; }
        public int CodServico { get; set; }
        public string Servico { get; set; }
        public int CodCliente { get; set; }
        public string NomeCliente { get; set; }
        public string HoraInicio { get; set; }
        public string HoraTermino { get; set; }
        public int CodStatus { get; set; }
        public String Status { get; set; }
        
        /// <summary>
        /// Este é o valor do serviço no dia que o procedimento é realizado
        /// </summary>
        public decimal? ValorServico { get; set; }

        /// <summary>
        /// Propriedades do scheduler
        /// </summary>
        public string start { get { return this.Inicio.ToString("yyyy-MM-dd HH':'mm':'ss"); } }
        public string end { get { return this.Termino.ToString("yyyy-MM-dd HH':'mm':'ss"); } }
        public string id { get { return this.CodAgendamento.ToString(); } }
        public string horarioLabel { get { return this.Inicio.ToString("HH':'mm") + " - " + this.Termino.ToString("HH':'mm"); } }
        public string title 
        { 
            get 
            {
                return this.NomeCliente + "\n" +
                    this.NomeProfissional + "n"+
                    this.Servico + "\n" +
                    this.NomeProfissional; 
            } 
        }

        internal void LoadFromAgendamento(AgendamentoItem e)
        {
            this.CodAgendamento = e.Cd_Agendamento;
            this.CodAgendamentoItem = e.Cd_AgendamentoItem;
            this.CodServico = e.Cd_Servico;
            this.CodCliente = e.Agendamento.Cd_Cliente;
            this.CodProfissional = e.Profissional.Cod_Pessoa;
            this.Inicio = e.Dat_Inicio;
            this.Termino = e.Dat_Termino;
            this.DataAgendamento = e.Agendamento.Dat_Agendamento.ToString("yyyy-MM-dd");
            this.HoraInicio = e.Dat_Inicio.ToString("HH':'mm");
            this.HoraTermino = e.Dat_Termino.ToString("HH':'mm");
            this.CodStatus = e.Agendamento.Cd_Status;
            this.NomeCliente = e.Agendamento.Cliente.Txt_Nome;
            this.NomeProfissional = e.Profissional.Txt_Nome;
            this.Servico = e.Servico.Nome_Servico;
            this.ValorServico = (decimal)(e.Num_ValorServico ?? 0);

            switch (this.CodStatus)
            {
                case 1:
                    this.Status = "Agendado";
                    break;
                case 2:
                    this.Status = "Cancelado";
                    break;
                case 3:
                    this.Status = "Realizado";
                    break;
                default:
                    break;
            }
        }

        internal AgendamentoItem ToAgendamento()
        {
            var temp = new AgendamentoItem();

            temp.Cd_Agendamento = this.CodAgendamento;
            temp.Cd_AgendamentoItem = this.CodAgendamentoItem;
            temp.Cd_Servico = this.CodServico;
            temp.Dat_Inicio = this.Inicio;
            temp.Dat_Termino = this.Termino;
            temp.Cd_Profissional = this.CodProfissional;
            temp.Num_ValorServico = this.ValorServico;

            return temp;
        }
    }
}
