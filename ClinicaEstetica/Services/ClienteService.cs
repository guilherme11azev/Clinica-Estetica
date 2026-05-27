using ClinicaEstetica.DTOs;
using ClinicaEstetica.Models;
using ClinicaEstetica.Repositories;

namespace ClinicaEstetica.Services;

public class ClienteService
{
    private readonly ClienteRepository _repository;

    public ClienteService(ClienteRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ClienteResponseDTO>> GetAllAsync()
    {
        var clientes = await _repository.GetAllAsync();
        return clientes.Select(MapToResponse).ToList();
    }

    public async Task<ClienteResponseDTO?> GetByIdAsync(int id)
    {
        var cliente = await _repository.GetByIdAsync(id);
        return cliente == null ? null : MapToResponse(cliente);
    }

    public async Task<List<AgendamentoResponseDTO>> GetHistoricoAsync(int id)
    {
        var cliente = await _repository.GetByIdAsync(id);
        if (cliente == null)
            throw new KeyNotFoundException("Cliente não encontrado");

        var agendamentos = await _repository.GetHistoricoAsync(id);
        return agendamentos.Select(MapAgendamentoToResponse).ToList();
    }

    public async Task<ClienteResponseDTO> CreateAsync(ClienteCreateDTO dto)
    {
        var cliente = new Cliente
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Telefone = dto.Telefone,
            DataNascimento = dto.DataNascimento,
            Observacoes = dto.Observacoes
        };

        var criado = await _repository.CreateAsync(cliente);
        return MapToResponse(criado);
    }

    public async Task<ClienteResponseDTO?> UpdateAsync(int id, ClienteUpdateDTO dto)
    {
        var cliente = await _repository.GetByIdAsync(id);
        if (cliente == null) return null;

        cliente.Nome = dto.Nome;
        cliente.Email = dto.Email;
        cliente.Telefone = dto.Telefone;
        cliente.Observacoes = dto.Observacoes;

        var atualizado = await _repository.UpdateAsync(cliente);
        return MapToResponse(atualizado);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static ClienteResponseDTO MapToResponse(Cliente c) => new()
    {
        Id = c.Id,
        Nome = c.Nome,
        Email = c.Email,
        Telefone = c.Telefone,
        DataNascimento = c.DataNascimento,
        Observacoes = c.Observacoes
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