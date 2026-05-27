namespace ClinicaEstetica.Models;

public class Procedimento
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public int DuracaoMinutos { get; set; }
    public decimal Preco { get; set; }

    // Propriedade de navegação — um procedimento tem muitos agendamentos
    public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
}