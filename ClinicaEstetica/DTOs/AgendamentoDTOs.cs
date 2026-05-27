using System.ComponentModel.DataAnnotations;
using ClinicaEstetica.Models;

namespace ClinicaEstetica.DTOs;

public class AgendamentoCreateDTO
{
    [Required(ErrorMessage = "Data e hora são obrigatórias")]
    public DateTime DataHora { get; set; }

    [Required(ErrorMessage = "BiomedicoId é obrigatório")]
    public int BiomedicoId { get; set; }

    [Required(ErrorMessage = "ClienteId é obrigatório")]
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "ProcedimentoId é obrigatório")]
    public int ProcedimentoId { get; set; }

    [StringLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres")]
    public string? Observacoes { get; set; }
}

public class AgendamentoCancelarDTO
{
    [Required(ErrorMessage = "Motivo do cancelamento é obrigatório")]
    [StringLength(500, ErrorMessage = "Motivo deve ter no máximo 500 caracteres")]
    public string Motivo { get; set; } = string.Empty;
}

public class AgendamentoResponseDTO
{
    public int Id { get; set; }
    public DateTime DataHora { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Observacoes { get; set; }

    // Dados resumidos das entidades relacionadas
    public int BiomedicoId { get; set; }
    public string BiomedicoNome { get; set; } = string.Empty;

    public int ClienteId { get; set; }
    public string ClienteNome { get; set; } = string.Empty;

    public int ProcedimentoId { get; set; }
    public string ProcedimentoNome { get; set; } = string.Empty;
    public int DuracaoMinutos { get; set; }
    public decimal Preco { get; set; }
}

public class ResumoDiaDTO
{
    public DateTime Data { get; set; }
    public int TotalAgendamentos { get; set; }
    public int Agendados { get; set; }
    public int Confirmados { get; set; }
    public int Concluidos { get; set; }
    public int Cancelados { get; set; }
    public List<AgendamentoResponseDTO> Agendamentos { get; set; } = new();
}