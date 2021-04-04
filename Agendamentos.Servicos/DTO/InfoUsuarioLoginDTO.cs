using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos.DTO
{
    public class InfoUsuarioLoginDTO
    {
        public string NomeUsuario { get; set; }
        public bool Administrador { get; set; }
        public bool Funcionario { get; set; }
        public int CodPessoa { get; set; }
    }
}
