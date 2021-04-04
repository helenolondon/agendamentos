using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Agendamentos.Infra.Modelos
{
    public class UsuariosGrupos
    {
        [Key]
        public int Cd_UsuarioGrupo { get; set; }
        public int Cd_Grupo { get; set; }
        public int Cd_Usuario { get; set; }

        [ForeignKey("Cd_Usuario")]
        public Usuario Usuario { get; set; }
        
        [ForeignKey("Cd_Grupo")]
        public Grupo Grupo { get; set; }
    }
}
