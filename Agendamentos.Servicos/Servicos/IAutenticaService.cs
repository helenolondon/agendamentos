using Agendamentos.Servicos.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos.Servicos
{
    public interface IAutenticaService
    {
        bool TokenValido(string auth, int codUsuartio);
        InfoUsuarioLoginDTO ObterInfoUsuario(int codUsuario);
    }
}
