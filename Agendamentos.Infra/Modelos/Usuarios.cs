using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Agendamentos.Infra.Modelos
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int Cod_Usuario { get; set; }
        public string Nome { get; set; }
        public int? Cod_Pessoa { get; set; }
        public List<UsuariosGrupos> Grupos { get; set; }
    }
}
