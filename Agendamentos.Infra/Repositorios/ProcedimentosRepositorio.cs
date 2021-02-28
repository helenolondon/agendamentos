using Agendamentos.Infra.EF;
using Agendamentos.Infra.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agendamentos.Infra.Repositorios
{
    public class ProcedimentosRepositorio: IProcedimentosRepositorio
    {
        private readonly AgendamentosDbContext dbContext;
        public ProcedimentosRepositorio(AgendamentosDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<Procedimento> ListarProcedimentos(int codProfissional)
        {
            return this.dbContext.Procedimentos
                .Where(p => p.Cd_Pessoa == codProfissional)
                .Include(t => t.Pessoa)
                .Include(s => s.Servico)
                .OrderBy(h => h.Num_HoraInicio)
                .ToList();
        }

        public List<Procedimento> ListarPorProfissionais(List<Pessoa> pessoas)
        {
            var q = this.dbContext.Procedimentos.Where(p => pessoas.Contains(p.Pessoa)).ToList();

            if (q != null && q.Count() > 0)
            {
                return q;
            }

            return null;
        }

        public decimal ObterComissao(DateTime data, TimeSpan inicio, TimeSpan termino, int codServico)
        {
            var diaSemana = 0;

            switch (data.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    diaSemana = 1;
                    break;
                case DayOfWeek.Monday:
                    diaSemana = 2;
                    break;
                case DayOfWeek.Tuesday:
                    diaSemana = 3;
                    break;
                case DayOfWeek.Wednesday:
                    diaSemana = 4;
                    break;
                case DayOfWeek.Thursday:
                    diaSemana = 5;
                    break;
                case DayOfWeek.Friday:
                    diaSemana = 6;
                    break;
                case DayOfWeek.Saturday:
                    diaSemana = 7;
                    break;
                default:
                    break;
            }

            var procedimentos = this.ListarPorProfissionais(diaSemana, inicio, termino, codServico);

            var pComissTodosOsDias = procedimentos.Where(p => p.Cd_DiaSemana == 0).FirstOrDefault();
            var pComissDia = procedimentos.Where(p => p.Cd_DiaSemana == diaSemana).FirstOrDefault();

            if (pComissDia != null)
            {
                return pComissDia.Num_Comissao;
            };
            
            if (pComissTodosOsDias != null)
            {
                return pComissTodosOsDias.Num_Comissao;
            };

            return 0;
        }


        public List<Procedimento> ListarPorProfissionais(int diaSemana, TimeSpan inicio, TimeSpan termino, int codServico)
        {
            var procedimentos = this.dbContext.Procedimentos
                .Include(r => r.Pessoa)
                .Where(p => p.Cd_DiaSemana == diaSemana || p.Cd_DiaSemana == 0)
                .Where(p => p.Cd_Servico == codServico)
                .Where(p => p.Num_HoraInicio <= inicio && p.Num_HoraFim >= termino)
                .ToList();

            if(procedimentos == null || procedimentos.Count == 0)
            {
                return null;
            }
            
            return procedimentos;
        }
    }
}
