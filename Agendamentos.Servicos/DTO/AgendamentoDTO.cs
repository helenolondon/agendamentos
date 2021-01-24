using Agendamentos.Infra.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos.DTO
{
    /// <summary>
    /// Representação dos dados de um agendamento
    /// </summary>
    public class AgendamentoDTO
    {
        public int CodAgendamento { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Termino { get; set; }
        public int CodProcedimento { get; set; }
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

        internal void LoadFromAgendamento(AgendamentoProfissional e)
        {
            this.CodAgendamento = e.Cd_Agendamento;
            this.CodProcedimento = e.Cd_Procedimento;
            this.Inicio = e.Data_Inicio;
            this.Termino = e.Data_Termino;

            this.NomeCliente = "Cliente não informado";
            this.NomeProfissional = e.Procedimento.Pessoa.Txt_Nome;
            this.Servico = e.Procedimento.Servico.Nome_Servico;
        }
    }
}
