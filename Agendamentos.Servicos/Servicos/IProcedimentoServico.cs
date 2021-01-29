using Agendamentos.Servicos.Listas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos
{
    public interface IProcedimentoServico
    {
        ProcedimentoListaDTO ListaPorPessoa(int codPessoa);
        PessoasListaDTO ListarProfissionaisParaAgendamento(int diaSemana, TimeSpan inicio, TimeSpan termino, int codServico);
    }
}
