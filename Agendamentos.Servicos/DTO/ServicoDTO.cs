using Agendamentos.Infra.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos.DTO
{
    public class ServicoDTO
    {
        public int CodServico { get; set; }
        public string NomeServico { get; set; }
        internal void LoadFromServico(Servico servico)
        {
            this.CodServico = servico.Id_Servico;
            this.NomeServico = servico.Nome_Servico;
        }
        internal Servico ToServico()
        {
            var temp = new Servico();

            temp.Id_Servico = this.CodServico;
            temp.Nome_Servico = this.NomeServico;

            return temp;
        }
    }
}
