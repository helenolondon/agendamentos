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
        public DbSet<PessoasCategorias> Categorias { get; set; }
        public DbSet<AgendamentosConfiguracoes> Configuracoes { get; set; }
        public DbSet<Compromisso> Compromissos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuariosGrupos> UsuariosGrupos { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<TokensAcesso> TokensAcesso { get; set; }

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
            modelBuilder.Entity<AgendamentoItem>()
                .HasOne(a => a.Agendamento)
                .WithMany(a => a.Itens);
            
            modelBuilder.Entity<PessoasCategorias>()
            .HasKey(c => new { c.Cd_Categoria, c.Cd_Pessoa});
        }
    }
}
