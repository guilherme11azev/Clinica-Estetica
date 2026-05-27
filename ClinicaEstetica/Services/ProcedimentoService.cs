using ClinicaEstetica.DTOs;
using ClinicaEstetica.Models;
using ClinicaEstetica.Repositories;

namespace ClinicaEstetica.Services;

public class ProcedimentoService
{
    private readonly ProcedimentoRepository _repository;

    public ProcedimentoService(ProcedimentoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ProcedimentoResponseDTO>> GetAllAsync()
    {
        var procedimentos = await _repository.GetAllAsync();
        return procedimentos.Select(MapToResponse).ToList();
    }

    public async Task<ProcedimentoResponseDTO?> GetByIdAsync(int id)
    {
        var procedimento = await _repository.GetByIdAsync(id);
        return procedimento == null ? null : MapToResponse(procedimento);
    }

    public async Task<ProcedimentoResponseDTO> CreateAsync(ProcedimentoCreateDTO dto)
    {
        var procedimento = new Procedimento
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            DuracaoMinutos = dto.DuracaoMinutos,
            Preco = dto.Preco
        };

        var criado = await _repository.CreateAsync(procedimento);
        return MapToResponse(criado);
    }

    public async Task<ProcedimentoResponseDTO?> UpdateAsync(int id, ProcedimentoUpdateDTO dto)
    {
        var procedimento = await _repository.GetByIdAsync(id);
        if (procedimento == null) return null;

        procedimento.Nome = dto.Nome;
        procedimento.Descricao = dto.Descricao;
        procedimento.DuracaoMinutos = dto.DuracaoMinutos;
        procedimento.Preco = dto.Preco;

        var atualizado = await _repository.UpdateAsync(procedimento);
        return MapToResponse(atualizado);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static ProcedimentoResponseDTO MapToResponse(Procedimento p) => new()
    {
        Id = p.Id,
        Nome = p.Nome,
        Descricao = p.Descricao,
        DuracaoMinutos = p.DuracaoMinutos,
        Preco = p.Preco
    };
}