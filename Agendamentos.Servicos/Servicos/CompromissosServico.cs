using Agendamentos.Infra;
using Agendamentos.Servicos.DTO;
using Agendamentos.Servicos.Erros;
using Agendamentos.Servicos.Listas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos.Servicos
{
    public class CompromissosServico : ServicoBase, ICompromissosServico
    {
        public CompromissosServico(IRepositorios repositorio) : base(repositorio)
        {
        }
        public void Excluir(int codCompromisso)
        {
            this.repositorio.CompromissosRepositorio.Excluir(codCompromisso);
        }

        public CompromissoDTO ObterCompromisso(int codCompromisso)
        {
            var comp = this.repositorio.CompromissosRepositorio.ObterCompromisso(codCompromisso);

            if(comp == null)
            {
                return null;
            }

            var dto = new CompromissoDTO();
            dto.LoadFromCompromisso(comp);

            return dto;
        }

        public CompromissosListaDTO ObterCompromissosPorProfissional(int codProfissional, DateTime data)
        {
            var comp = this.repositorio.CompromissosRepositorio.ObterCompromissosPorProfissional(codProfissional, data);

            if (comp == null || comp.Count == 0)
            {
                return null;
            }

            var dto = new CompromissosListaDTO();
            dto.LoadFromCompromissos(comp);

            return dto;
        }

        private void ValidarCompromisso(CompromissoDTO comp)
        {
            if(comp == null)
            {
                throw new ServicosException("Compromisso não pode ser nulo");
            }

            if(comp.CodCompromisso < 0)
            {
                throw new ServicosException("Código do compromisso deve ser maior ou igual a zero");
            }

            if(!(comp.CodProfissional > 0))
            {
                throw new ServicosException("Código do profissinal deve ser maior do que zero");
            }

            if(comp.CodTipo < 1 || comp.CodTipo > 3)
            {
                throw new ServicosException("Tipo de compromisso inválido");
            }

            if (comp.Inicio == null || comp.Termino == null)
            {
                throw new ServicosException("É necessário informar o início e o término do compromisso");
            }
        }

        public int Salvar(CompromissoDTO compromisso)
        {
            ValidarCompromisso(compromisso);

            return this.repositorio.CompromissosRepositorio.Salvar(compromisso.ToCompromisso());
        }
    }
}
