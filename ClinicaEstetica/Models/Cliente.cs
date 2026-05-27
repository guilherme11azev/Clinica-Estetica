namespace ClinicaEstetica.Models;

public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string? Observacoes { get; set; }

    public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
}