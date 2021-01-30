using Agendamentos.Infra.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Agendamentos.Infra.EF
{
    public class AgendamentosDbContext: DbContext
    {
        private readonly string connectionString;
        public  DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Procedimento> Procedimentos { get; set; }
        public DbSet<AgendamentoItem> AgendamentoItens { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }

        public AgendamentosDbContext(string connectionString)
        {
            this.connectionString = connectionString;
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public AgendamentosDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<LoginODP>()
            //    .HasOne<UsuarioODP>(l => l.UsuarioCadastro)
            //    .WithMany()
            //    .HasForeignKey(l => l.UsuarioCad)
            //    .HasPrincipalKey(u => u.Login);
        }
    }
}
