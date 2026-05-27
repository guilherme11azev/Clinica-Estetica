using ClinicaEstetica.DTOs;
using ClinicaEstetica.Models;
using ClinicaEstetica.Repositories;

namespace ClinicaEstetica.Services;

public class BiomedicoService
{
    private readonly BiomedicoRepository _repository;

    public BiomedicoService(BiomedicoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<BiomedicoResponseDTO>> GetAllAsync()
    {
        var biomedicos = await _repository.GetAllAsync();
        return biomedicos.Select(MapToResponse).ToList();
    }

    public async Task<BiomedicoResponseDTO?> GetByIdAsync(int id)
    {
        var biomedico = await _repository.GetByIdAsync(id);
        return biomedico == null ? null : MapToResponse(biomedico);
    }

    public async Task<List<AgendamentoResponseDTO>> GetAgendaAsync(int id, DateTime inicio, DateTime fim)
    {
        var biomedico = await _repository.GetByIdAsync(id);
        if (biomedico == null)
            throw new KeyNotFoundException("Biomédico não encontrado");

        var agendamentos = await _repository.GetAgendaAsync(id, inicio, fim);
        return agendamentos.Select(MapAgendamentoToResponse).ToList();
    }

    public async Task<BiomedicoResponseDTO> CreateAsync(BiomedicoCreateDTO dto)
    {
        var biomedico = new Biomedico
        {
            Nome = dto.Nome,
            Email = dto.Email,
            CRBM = dto.CRBM,
            Especialidade = dto.Especialidade,
            Telefone = dto.Telefone,
            Ativo = true
        };

        var criado = await _repository.CreateAsync(biomedico);
        return MapToResponse(criado);
    }

    public async Task<BiomedicoResponseDTO?> UpdateAsync(int id, BiomedicoUpdateDTO dto)
    {
        var biomedico = await _repository.GetByIdAsync(id);
        if (biomedico == null) return null;

        biomedico.Nome = dto.Nome;
        biomedico.Email = dto.Email;
        biomedico.Especialidade = dto.Especialidade;
        biomedico.Telefone = dto.Telefone;
        biomedico.Ativo = dto.Ativo;

        var atualizado = await _repository.UpdateAsync(biomedico);
        return MapToResponse(atualizado);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static BiomedicoResponseDTO MapToResponse(Biomedico b) => new()
    {
        Id = b.Id,
        Nome = b.Nome,
        Email = b.Email,
        CRBM = b.CRBM,
        Especialidade = b.Especialidade,
        Telefone = b.Telefone,
        Ativo = b.Ativo
    };

    private static AgendamentoResponseDTO MapAgendamentoToResponse(Agendamento a) => new()
    {
        Id = a.Id,
        DataHora = a.DataHora,
        Status = a.Status.ToString(),
        Observacoes = a.Observacoes,
        BiomedicoId = a.BiomedicoId,
        BiomedicoNome = a.Biomedico.Nome,
        ClienteId = a.ClienteId,
        ClienteNome = a.Cliente.Nome,
        ProcedimentoId = a.ProcedimentoId,
        ProcedimentoNome = a.Procedimento.Nome,
        DuracaoMinutos = a.Procedimento.DuracaoMinutos,
        Preco = a.Procedimento.Preco
    };
}