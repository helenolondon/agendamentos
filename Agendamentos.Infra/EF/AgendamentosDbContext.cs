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
        public DbSet<AgendamentoProfissional> AgendamentoProfissionais { get; set; }
        public AgendamentosDbContext(string connectionString)
        {
            this.connectionString = connectionString;
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
            //    .HasOne<UsuarioODP>(l => l.UsuarioAlteracao)
            //    .WithMany()
            //    .HasForeignKey(l => l.UsuarioAlt)
            //    .HasPrincipalKey(u => u.Login);

            //modelBuilder.Entity<LoginODP>()
            //    .HasOne<UsuarioODP>(l => l.UsuarioCadastro)
            //    .WithMany()
            //    .HasForeignKey(l => l.UsuarioCad)
            //    .HasPrincipalKey(u => u.Login);
        }
    }
}
