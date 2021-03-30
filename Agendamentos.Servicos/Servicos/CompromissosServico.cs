using Agendamentos.Infra;
using Agendamentos.Infra.Modelos;
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

        public CompromissosListaDTO ObterCompromissos(DateTime inicio, DateTime termino, int codProfissional)
        {
            var comps = this.repositorio.CompromissosRepositorio
                .ObterCompromissosPorProfissional(codProfissional, inicio, termino, false);

            if(comps == null || comps.Count == 0)
            {
                return null;
            }

            var dtos = new CompromissosListaDTO();
            dtos.LoadFromCompromissos(comps);

            return dtos;
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
            var comp = this.repositorio.CompromissosRepositorio.ObterCompromissosPorProfissional(codProfissional, data, data, false);

            if (comp == null || comp.Count == 0)
            {
                return null;
            }

            var dto = new CompromissosListaDTO();
            dto.LoadFromCompromissos(comp);

            return dto;
        }

        public CompromissosListaDTO ObterCompromissosPorProfissional(int codProfissional, DateTime inicio, DateTime termino, bool permitirTodos)
        {
            var comp = this.repositorio.CompromissosRepositorio.ObterCompromissosPorProfissional(codProfissional, inicio, termino, permitirTodos);

            if (comp == null)
            {
                return new CompromissosListaDTO(); 
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

            var conflitaAlmoco = false;
            var foraDeHorario = false;

            var valido = true;
            var conf = this.repositorio.AgendamentosRepositorio.ObterConfiguracoes();
            List<Compromisso> comps;

            var inicioAlmoco = comp.Inicio.Date + conf.Tim_AlmocInicio;
            var fimAlmoco = comp.Inicio.Date + conf.Tim_AlmocFinal;

            var inicioExpediente = comp.Inicio.Date + conf.Tim_FuncInicio;
            var fimExpediente = comp.Inicio.Date + conf.Tim_FuncFinal;

            if (conf != null)
            {
                foraDeHorario = comp.Inicio < inicioExpediente;
                foraDeHorario = foraDeHorario || comp.Termino > fimExpediente;

                if (conf.Num_BloqAlmoco == 1)
                {
                    conflitaAlmoco = inicioAlmoco >= comp.Inicio && inicioAlmoco < comp.Termino;
                    conflitaAlmoco = conflitaAlmoco || (fimAlmoco > comp.Inicio && fimAlmoco <= comp.Termino);
                    conflitaAlmoco = conflitaAlmoco || (inicioAlmoco < comp.Inicio && fimAlmoco > comp.Termino);
                }
            }

            if (foraDeHorario)
            {
                throw new ServicosException("Compromisso fora do horário permitido");
            }

            if (conflitaAlmoco)
            {
                throw new ServicosException("Não é permitido marcar compromisso durante o horário de almoço");
            }

            if (comp.CodCompromisso == 0)
            {
                // valida conflitos com compromissos
                comps = this.repositorio.CompromissosRepositorio.ListarCompromissos(a => a.Dat_Inicio >= comp.Inicio && a.Dat_Inicio <= comp.Termino &&
                    a.Cd_Pessoa == comp.CodProfissional);

                valido &= comps == null || comps.Count == 0;

                comps = this.repositorio.CompromissosRepositorio.ListarCompromissos(a => a.Dat_Termino > comp.Inicio && a.Dat_Termino <= comp.Termino &&
                    a.Cd_Pessoa == comp.CodProfissional);

                valido &= comps == null || comps.Count == 0;

                comps = this.repositorio.CompromissosRepositorio.ListarCompromissos(a => a.Dat_Inicio < comp.Inicio && a.Dat_Termino > comp.Termino &&
                    a.Cd_Pessoa == comp.CodProfissional);

                valido &= comps == null || comps.Count == 0;
            }
            else
            {
                comps = this.repositorio.CompromissosRepositorio.ListarCompromissos(a => a.Dat_Inicio >= comp.Inicio && a.Dat_Inicio <= comp.Termino &&
                    a.Cd_Pessoa == comp.CodProfissional && comp.CodCompromisso != a.Cd_Compromisso);

                valido &= comps == null || comps.Count == 0;

                comps = this.repositorio.CompromissosRepositorio.ListarCompromissos(a => a.Dat_Termino > comp.Inicio && a.Dat_Termino <= comp.Termino &&
                    a.Cd_Pessoa == comp.CodProfissional && comp.CodCompromisso != a.Cd_Compromisso);

                valido &= comps == null || comps.Count == 0;

                comps = this.repositorio.CompromissosRepositorio.ListarCompromissos(a => a.Dat_Inicio < comp.Inicio && a.Dat_Termino > comp.Termino &&
                    a.Cd_Pessoa == comp.CodProfissional && comp.CodCompromisso != a.Cd_Compromisso);

                valido &= comps == null || comps.Count == 0;
            }

            if (!valido)
            {
                var nome = this.repositorio.PessoasRepositorio.Obter(comp.CodProfissional)?.Txt_Nome ?? "profissional não identificado";
                nome = nome.Split(" ")[0];

                throw new ServicosException(nome + " já possui compromisso no horário " + comp.horarioLabel);
            }
        }

        public int Salvar(CompromissoDTO compromisso)
        {
            ValidarCompromisso(compromisso);

            return this.repositorio.CompromissosRepositorio.Salvar(compromisso.ToCompromisso());
        }
    }
}
