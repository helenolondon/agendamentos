using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Agendamentos.Infra.Modelos
{
    public class Servico
    {
        [Key]
        public int Id_Servico { get; set; }
        public string Nome_Servico { get; set; }
    }
}
