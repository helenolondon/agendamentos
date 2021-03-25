using Agendamentos.Infra.EF;
using Agendamentos.Infra.Erros;
using Agendamentos.Infra.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agendamentos.Infra.Repositorios
{
    public class CompromissosRepositorio : ICompromissosRepositorio
    {
        private readonly AgendamentosDbContext dbContext;
        public CompromissosRepositorio(AgendamentosDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Excluir(int codCompromisso)
        {
            var comp = this.dbContext.Compromissos.Find(codCompromisso);

            if(comp == null)
            {
                throw new NaoEncontradoException("Compromisso não encontrado");
            }

            this.dbContext.Compromissos.Remove(comp);
            
            this.dbContext.SaveChanges();
        }

        public Compromisso ObterCompromisso(int codCompromisso)
        {
            return this.dbContext
                .Compromissos
                .Where(c => c.Cd_Compromisso == codCompromisso)
                .Include(c => c.Pessoa)
                .FirstOrDefault();
        }

        public List<Compromisso> ObterCompromissosPorProfissional(int codProfissional)
        {
            var comps = this.dbContext
                .Compromissos
                .Include(c => c.Pessoa)
                .Where(comp => comp.Cd_Pessoa == codProfissional)
                .ToList();

            if(comps != null && comps.Count == 0)
            {
                return null;
            }

            return comps;
        }

        public int Salvar(Compromisso compromisso)
        {
            if(compromisso.Cd_Compromisso == 0)
            {
                dbContext.Compromissos.Attach(compromisso);
            }
            else
            {
                if(this.dbContext.Compromissos.Count(c => c.Cd_Compromisso == compromisso.Cd_Compromisso) == 0)
                {
                    return 0;
                }

                dbContext.Compromissos.Update(compromisso);
            }
            
            dbContext.SaveChanges();

            return compromisso.Cd_Compromisso;
        }
    }
}
