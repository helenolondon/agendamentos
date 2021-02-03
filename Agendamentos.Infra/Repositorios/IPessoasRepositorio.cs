﻿using Agendamentos.Infra.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Infra.Repositorios
{
    public interface IPessoasRepositorio
    {
        List<Pessoa> Listar();
        Pessoa Obter(int codPessoa);
    }
}
