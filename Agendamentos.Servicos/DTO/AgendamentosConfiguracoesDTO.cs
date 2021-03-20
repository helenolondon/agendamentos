using Agendamentos.Infra.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos.DTO
{
    public class AgendamentosConfiguracoesDTO
    {
        /// <summary>
        /// Classe que representas as conffigurações da agenda
        /// </summary>
        public string FuncInicio { get; set; }
        public string FuncFinal { get; set; }
        public string AlmocInicio { get; set; }
        public string AlmocFinal { get; set; }
        public int BloqAlmoco { get; set; }
        public int DispSegunda { get; set; }
        public int DispTerca { get; set; }
        public int DispQuarta { get; set; }
        public int DispQuinta { get; set; }
        public int DispSexta { get; set; }
        public int DispSabado { get; set; }
        public int DispDomingo { get; set; }

        public void LoadFromConfiguracoes(AgendamentosConfiguracoes configuracoes)
        {
            this.FuncInicio = configuracoes.Tim_FuncInicio.ToString(@"hh\:mm");
            this.FuncFinal = configuracoes.Tim_FuncFinal.ToString(@"hh\:mm");

            this.AlmocInicio = configuracoes.Tim_AlmocInicio.ToString(@"hh\:mm");
            this.AlmocFinal = configuracoes.Tim_AlmocFinal.ToString(@"hh\:mm");
            this.BloqAlmoco = configuracoes.Num_BloqAlmoco;

            this.DispSegunda = configuracoes.Num_DispSegunda;
            this.DispTerca = configuracoes.Num_DispTerca;
            this.DispQuarta = configuracoes.Num_DispQuarta;
            this.DispQuinta = configuracoes.Num_DispQuinta;
            this.DispSexta = configuracoes.Num_DispSexta;
            this.DispSabado = configuracoes.Num_DispSabado;
            this.DispDomingo = configuracoes.Num_DispDomingo;
        }

        public AgendamentosConfiguracoes ToConfiguracoes()
        {
            var conf = new AgendamentosConfiguracoes();

            conf.Tim_FuncInicio = TimeSpan.Parse(this.FuncInicio);
            conf.Tim_FuncFinal = TimeSpan.Parse(this.FuncFinal);

            conf.Tim_AlmocInicio = TimeSpan.Parse(this.AlmocInicio);
            conf.Tim_AlmocFinal = TimeSpan.Parse(this.AlmocFinal);
            conf.Num_BloqAlmoco = this.BloqAlmoco;

            conf.Num_DispSegunda = this.DispSegunda;
            conf.Num_DispTerca = this.DispTerca;
            conf.Num_DispQuarta = this.DispQuarta;
            conf.Num_DispQuinta = this.DispQuinta;
            conf.Num_DispSexta = this.DispSexta;
            conf.Num_DispSabado = this.DispSabado;
            conf.Num_DispDomingo = this.DispDomingo;
        
            return conf;
        }
    }
}
