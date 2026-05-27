namespace ClinicaEstetica.Models;

public class Biomedico
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CRBM { get; set; } = string.Empty;
    public string Especialidade { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;

    // Propriedade de navegação — um biomédico tem muitos agendamentos
    public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
}