using Agendamentos.Infra.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Infra.Repositorios
{
    public interface ICompromissosRepositorio
    {
        List<Compromisso> ObterCompromissosPorProfissional(int codProfissional);
        Compromisso ObterCompromisso(int codCompromisso);
        void Excluir(int codCompromisso);
        int Salvar(Compromisso compromisso);
    }
}
