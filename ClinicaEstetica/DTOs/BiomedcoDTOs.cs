using System.ComponentModel.DataAnnotations;

namespace ClinicaEstetica.DTOs;

public class BiomedicoCreateDTO
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "CRBM é obrigatório")]
    [StringLength(20, ErrorMessage = "CRBM deve ter no máximo 20 caracteres")]
    public string CRBM { get; set; } = string.Empty;

    [Required(ErrorMessage = "Especialidade é obrigatória")]
    [StringLength(100, ErrorMessage = "Especialidade deve ter no máximo 100 caracteres")]
    public string Especialidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefone é obrigatório")]
    [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
    public string Telefone { get; set; } = string.Empty;
}

public class BiomedicoUpdateDTO
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Especialidade é obrigatória")]
    [StringLength(100, ErrorMessage = "Especialidade deve ter no máximo 100 caracteres")]
    public string Especialidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefone é obrigatório")]
    [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
    public string Telefone { get; set; } = string.Empty;

    public bool Ativo { get; set; }
}

public class BiomedicoResponseDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CRBM { get; set; } = string.Empty;
    public string Especialidade { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public bool Ativo { get; set; }
}