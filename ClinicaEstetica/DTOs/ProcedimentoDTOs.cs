using System.ComponentModel.DataAnnotations;

namespace ClinicaEstetica.DTOs;

public class ProcedimentoCreateDTO
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
    public string? Descricao { get; set; }

    [Required(ErrorMessage = "Duração é obrigatória")]
    [Range(1, 480, ErrorMessage = "Duração deve ser entre 1 e 480 minutos")]
    public int DuracaoMinutos { get; set; }

    [Required(ErrorMessage = "Preço é obrigatório")]
    [Range(0.01, 99999.99, ErrorMessage = "Preço deve ser maior que zero")]
    public decimal Preco { get; set; }
}

public class ProcedimentoUpdateDTO
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
    public string? Descricao { get; set; }

    [Required(ErrorMessage = "Duração é obrigatória")]
    [Range(1, 480, ErrorMessage = "Duração deve ser entre 1 e 480 minutos")]
    public int DuracaoMinutos { get; set; }

    [Required(ErrorMessage = "Preço é obrigatório")]
    [Range(0.01, 99999.99, ErrorMessage = "Preço deve ser maior que zero")]
    public decimal Preco { get; set; }
}

public class ProcedimentoResponseDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public int DuracaoMinutos { get; set; }
    public decimal Preco { get; set; }
}