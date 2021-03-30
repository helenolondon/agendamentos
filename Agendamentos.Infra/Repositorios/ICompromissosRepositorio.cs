using Agendamentos.Infra.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Infra.Repositorios
{
    public interface ICompromissosRepositorio
    {
        List<Compromisso> ListarCompromissos(System.Linq.Expressions.Expression<Func<Compromisso, bool>> filtro);
        List<Compromisso> ObterCompromissosPorProfissional(int codProfissional, DateTime inicio, DateTime termino, bool permitirTodos);
        Compromisso ObterCompromisso(int codCompromisso);
        void Excluir(int codCompromisso);
        int Salvar(Compromisso compromisso);
    }
}
