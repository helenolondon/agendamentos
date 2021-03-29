using Agendamentos.Infra.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos.DTO
{
    /// <summary>
    /// Representa um compromisso de um profissional
    /// Compromissos bloqueam a agenda
    /// </summary>
    public class CompromissoDTO
    {
        public int CodCompromisso { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Termino { get; set; }
        public int CodTipo { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get { return ObterTipoCompromisso(this.CodTipo); } }
        public int CodProfissional { get; set; }
        public string NomeProfissional { get; set; }

        /// <summary>
        /// Propriedades do scheduler
        /// </summary>
        public string start { get { return this.Inicio.ToString("yyyy-MM-dd HH':'mm':'ss"); } }
        public string end { get { return this.Termino.ToString("yyyy-MM-dd HH':'mm':'ss"); } }
        public string id { get { return this.CodCompromisso.ToString(); } }
        public string horarioLabel { get { return this.Inicio.ToString("dd'/'MM HH':'mm") + " - " + this.Termino.ToString("dd'/'MM HH':'mm"); } }
        public string title
        {
            get
            {
                return this.Descricao + "\n" +
                    this.NomeProfissional + "n";
            }
        }

        public string Color
        {
            get
            {
                return "#ffe6ff";
            }
        }

        private string ObterTipoCompromisso(int codTipo)
        {
            switch (codTipo)
            {
                case 1:
                    return "Particular";
                case 2:
                    return "Fechado";
                case 3:
                    return "Normal";
                default:
                    return null;
            }
        }

        public void LoadFromCompromisso(Compromisso compromisso)
        {
            this.CodCompromisso = compromisso.Cd_Compromisso;
            this.Inicio = compromisso.Dat_Inicio;
            this.Termino = compromisso.Dat_Termino;
            this.CodTipo = compromisso.Cd_Tipo;
            this.CodProfissional = compromisso.Cd_Pessoa;
            this.NomeProfissional = compromisso.Pessoa?.Txt_Nome;
            this.Descricao = compromisso.Descricao;
        }

        public Compromisso ToCompromisso()
        {
            var comp = new Compromisso();

            comp.Cd_Compromisso = this.CodCompromisso;
            comp.Cd_Pessoa = this.CodProfissional;
            comp.Cd_Tipo = this.CodTipo;
            comp.Dat_Inicio = this.Inicio;
            comp.Dat_Termino = this.Termino;
            comp.Pessoa = null;
            comp.Descricao = this.Descricao;

            return comp;
        }
    }
}
