using Agendamentos.Infra.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Infra.Repositorios
{
    public interface IClientesRepositorio
    {
        List<Pessoa> Listar();
    }
}
