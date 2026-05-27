using ClinicaEstetica.Models;

namespace ClinicaEstetica.Models;

public enum StatusAgendamento
{
    Agendado = 1,
    Confirmado = 2,
    Concluido = 3,
    Cancelado = 4
}

public class Agendamento
{
    public int Id { get; set; }
    public DateTime DataHora { get; set; }
    public StatusAgendamento Status { get; set; } = StatusAgendamento.Agendado;
    public string? Observacoes { get; set; }

    // Chaves estrangeiras
    public int BiomedicoId { get; set; }
    public int ClienteId { get; set; }
    public int ProcedimentoId { get; set; }

    // Propriedades de navegação — EF Core usa para fazer os JOINs
    public Biomedico Biomedico { get; set; } = null!;
    public Cliente Cliente { get; set; } = null!;
    public Procedimento Procedimento { get; set; } = null!;
}