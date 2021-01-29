using Agendamentos.Infra.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Infra.Repositorios
{
    public interface IProcedimentosRepositorio
    {
        List<Procedimento> ListarProcedimentos(int codProfiossional);
        List<Procedimento> ListarPorProfissionais(List<Pessoa> pessoas);
        List<Procedimento> ListarPorProfissionais(int diaSemana, TimeSpan inicio, TimeSpan termino, int codServico);
    }
}
