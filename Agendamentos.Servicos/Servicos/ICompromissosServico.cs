using Agendamentos.Servicos.DTO;
using Agendamentos.Servicos.Listas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos
{
    public interface ICompromissosServico
    {
        CompromissosListaDTO ObterCompromissosPorProfissional(int codProfissional, DateTime inicio, DateTime termino, bool permitirTodos);
        CompromissosListaDTO ObterCompromissosPorProfissional(int codProfissional, DateTime data);
        CompromissoDTO ObterCompromisso(int codCompromisso);
        void Excluir(int codCompromisso);
        int Salvar(CompromissoDTO compromisso);
    }
}
