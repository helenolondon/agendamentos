using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Agendamentos.Infra.Modelos
{
    /// <summary>
    /// Classe que representas as conffigurações da agenda
    /// </summary>
    [Table("AgendamentosConfiguracoes")]
    public class AgendamentosConfiguracoes
    {
        [Key]
        public int CodConfiguracao { get; set; }
        [Required]
        public TimeSpan Tim_FuncInicio { get; set; }
        [Required]
        public TimeSpan Tim_FuncFinal { get; set; }
        [Required]
        public TimeSpan Tim_AlmocInicio { get; set; }
        [Required]
        public TimeSpan Tim_AlmocFinal { get; set; }
        [Required]
        public int Num_BloqAlmoco { get; set; }
        [Required]
        public int Num_DispSegunda { get; set; }
        [Required]
        public int Num_DispTerca { get; set; }
        [Required]
        public int Num_DispQuarta { get; set; }
        [Required]
        public int Num_DispQuinta { get; set; }
        [Required]
        public int Num_DispSexta { get; set; }
        [Required]
        public int Num_DispSabado { get; set; }
        [Required]
        public int Num_DispDomingo { get; set; }
    }
}
