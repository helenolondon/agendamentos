using Agendamentos.Infra.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos.DTO
{
    public class ProcedimentoDTO
    {
        public int CodProcedimento { get; set; }
        public int CodPessoa { get; set; }
        public string NomePessoa { get; set; }
        public int CodServico { get; set; }
        public string NomeServico { get; set; }
        public int CodDiaSemana { get; set; }
        public string DiaDaSemana { get; set; }
        public String Num_HoraInicio { get; set; }
        public String Num_HoraFim { get; set; }

        internal void LoadFromProcedimento(Procedimento e)
        {
            this.CodPessoa = e.Cd_Pessoa;
            this.CodDiaSemana = e.Cd_DiaSemana;
            this.CodProcedimento = e.Cd_Procedimento;
            this.CodServico = e.Cd_Servico;
            this.NomePessoa = e.Pessoa?.Txt_Nome;
            this.NomeServico = e.Servico?.Nome_Servico;
            this.Num_HoraFim = e.Num_HoraFim.ToString();
            this.Num_HoraInicio = e.Num_HoraInicio.ToString();
            this.DiaDaSemana = RetDiaSemana(this.CodDiaSemana); 
        }

        private string RetDiaSemana(int dia)
        {
            switch (dia)
            {
                case 1:
                    return "Domingo";
                case 2:
                    return "Segunda";
                case 3:
                    return "Terça";
                case 4:
                    return "Quarta";
                case 5:
                    return "Quinta";
                case 6:
                    return "Sexta";
                case 7:
                    return "Sábado";
                case 0:
                    return "Todos os dias";
                default:
                    return null;
            }
        }

    }
}
