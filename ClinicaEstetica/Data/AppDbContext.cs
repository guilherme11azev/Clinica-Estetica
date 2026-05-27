using Microsoft.EntityFrameworkCore;
using ClinicaEstetica.Models;

namespace ClinicaEstetica.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Biomedico> Biomedicos { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Procedimento> Procedimentos { get; set; }
    public DbSet<Agendamento> Agendamentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Precisão monetária
        modelBuilder.Entity<Procedimento>()
            .Property(p => p.Preco)
            .HasPrecision(18, 2);

        // Seed — biomédicos iniciais
        modelBuilder.Entity<Biomedico>().HasData(
            new Biomedico { Id = 1, Nome = "Dra. Ana Paula", Email = "ana@clinica.com", CRBM = "CRBM-1234", Especialidade = "Estética Avançada", Telefone = "11999990001", Ativo = true },
            new Biomedico { Id = 2, Nome = "Dr. Carlos Lima", Email = "carlos@clinica.com", CRBM = "CRBM-5678", Especialidade = "Harmonização Facial", Telefone = "11999990002", Ativo = true }
        );

        // Seed — procedimentos iniciais
        modelBuilder.Entity<Procedimento>().HasData(
            new Procedimento { Id = 1, Nome = "Limpeza de Pele", Descricao = "Limpeza profunda com extração", DuracaoMinutos = 60, Preco = 180.00m },
            new Procedimento { Id = 2, Nome = "Botox", Descricao = "Aplicação de toxina botulínica", DuracaoMinutos = 30, Preco = 800.00m },
            new Procedimento { Id = 3, Nome = "Preenchimento Labial", Descricao = "Preenchimento com ácido hialurônico", DuracaoMinutos = 45, Preco = 1200.00m }
        );
    }
}