using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Agendamentos.Infra.Modelos
{
    [Table("ProfissionaisCompromissos")]
    public class Compromisso
    {
        [Key]
        public int CodCompromisso { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        public DateTime Dat_Inicio { get; set; }
        [Required]
        public DateTime Dat_Termino { get; set; }
        [Required]
        public int Cd_Tipo { get; set; }
        [Required]
        public int Cd_Pessoa { get; set; }
    }
}
