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
        public int CodAgendamentoItem { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Termino { get; set; }
        public int CodProfissional { get; set; }
        public string NomeProfissional { get; set; }
        public int CodServico { get; set; }
        public string Servico { get; set; }
        public string CodCliente { get; set; }
        public string NomeCliente { get; set; }
        
        /// <summary>
        /// Propriedades do scheduler
        /// </summary>
        public string start { get { return this.Inicio.ToString("yyyy-MM-dd HH':'mm':'ss"); } }
        public string end { get { return this.Termino.ToString("yyyy-MM-dd HH':'mm':'ss"); } }
        public string id { get { return this.CodAgendamento.ToString(); } }
        public string title 
        { 
            get 
            {
                return this.NomeCliente + "\n" +
                    this.Servico + "\n" +
                    this.NomeProfissional; 
            } 
        }

        internal void LoadFromAgendamento(AgendamentoItem e)
        {
            this.CodAgendamento = e.Cd_Agendamento;
            this.CodAgendamentoItem = e.Cd_AgendamentoItem;
            this.CodServico = e.Cd_Servico;
            this.Inicio = e.Dat_Inicio;
            this.Termino = e.Dat_Termino;

            this.NomeCliente = "Cliente não informado";
            this.NomeProfissional = e.Agendamento.Cliente.Txt_Nome;
            this.Servico = e.Servico.Nome_Servico;
        }

        internal AgendamentoItem ToAgendamento()
        {
            var temp = new AgendamentoItem();

            temp.Cd_Agendamento = this.CodAgendamento;
            temp.Cd_AgendamentoItem = this.CodAgendamentoItem;
            temp.Cd_Servico = this.CodServico;
            temp.Dat_Inicio = this.Inicio;
            temp.Dat_Termino = this.Termino;
            //temp.Servico = new Servico()
            //{
            //    Id_Servico = this.CodServico
            //};

            return temp;
        }
    }
}
