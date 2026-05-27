using ClinicaEstetica.DTOs;
using ClinicaEstetica.Models;
using ClinicaEstetica.Repositories;

namespace ClinicaEstetica.Services;

public class AgendamentoService
{
    private readonly AgendamentoRepository _agendamentoRepository;
    private readonly BiomedicoRepository _biomedicoRepository;
    private readonly ClienteRepository _clienteRepository;
    private readonly ProcedimentoRepository _procedimentoRepository;

    private const int AntecedenciaMinimaHoras = 2;

    public AgendamentoService(
        AgendamentoRepository agendamentoRepository,
        BiomedicoRepository biomedicoRepository,
        ClienteRepository clienteRepository,
        ProcedimentoRepository procedimentoRepository)
    {
        _agendamentoRepository = agendamentoRepository;
        _biomedicoRepository = biomedicoRepository;
        _clienteRepository = clienteRepository;
        _procedimentoRepository = procedimentoRepository;
    }

    public async Task<List<AgendamentoResponseDTO>> GetAllAsync(
        DateTime? data, int? biomedicoId, int? clienteId, StatusAgendamento? status)
    {
        var agendamentos = await _agendamentoRepository.GetAllAsync(data, biomedicoId, clienteId, status);
        return agendamentos.Select(MapToResponse).ToList();
    }

    public async Task<AgendamentoResponseDTO?> GetByIdAsync(int id)
    {
        var agendamento = await _agendamentoRepository.GetByIdAsync(id);
        return agendamento == null ? null : MapToResponse(agendamento);
    }

    public async Task<AgendamentoResponseDTO> CreateAsync(AgendamentoCreateDTO dto)
    {
        // Valida se biomédico existe e está ativo
        var biomedico = await _biomedicoRepository.GetByIdAsync(dto.BiomedicoId);
        if (biomedico == null)
            throw new KeyNotFoundException("Biomédico não encontrado");
        if (!biomedico.Ativo)
            throw new InvalidOperationException("Biomédico está inativo e não pode receber agendamentos");

        // Valida se cliente existe
        var cliente = await _clienteRepository.GetByIdAsync(dto.ClienteId);
        if (cliente == null)
            throw new KeyNotFoundException("Cliente não encontrado");

        // Valida se procedimento existe
        var procedimento = await _procedimentoRepository.GetByIdAsync(dto.ProcedimentoId);
        if (procedimento == null)
            throw new KeyNotFoundException("Procedimento não encontrado");

        // Valida se o horário é no futuro
        if (dto.DataHora <= DateTime.Now)
            throw new InvalidOperationException("O agendamento deve ser para uma data e hora futura");

        // Verifica conflito de horário
        var temConflito = await _agendamentoRepository.TemConflitoAsync(
            dto.BiomedicoId, dto.ClienteId, dto.DataHora, procedimento.DuracaoMinutos);

        if (temConflito)
            throw new InvalidOperationException(
                "Conflito de horário: o biomédico ou o cliente já possui agendamento nesse período");

        var agendamento = new Agendamento
        {
            DataHora = dto.DataHora,
            Status = StatusAgendamento.Agendado,
            Observacoes = dto.Observacoes,
            BiomedicoId = dto.BiomedicoId,
            ClienteId = dto.ClienteId,
            ProcedimentoId = dto.ProcedimentoId
        };

        var criado = await _agendamentoRepository.CreateAsync(agendamento);

        // Recarrega com os dados relacionados para montar o response
        var completo = await _agendamentoRepository.GetByIdAsync(criado.Id);
        return MapToResponse(completo!);
    }

    public async Task<AgendamentoResponseDTO> ConfirmarAsync(int id)
    {
        var agendamento = await _agendamentoRepository.GetByIdAsync(id);
        if (agendamento == null)
            throw new KeyNotFoundException("Agendamento não encontrado");

        // Só pode confirmar se estiver Agendado
        if (agendamento.Status != StatusAgendamento.Agendado)
            throw new InvalidOperationException(
                $"Não é possível confirmar um agendamento com status '{agendamento.Status}'");

        agendamento.Status = StatusAgendamento.Confirmado;
        await _agendamentoRepository.UpdateAsync(agendamento);
        return MapToResponse(agendamento);
    }

    public async Task<AgendamentoResponseDTO> ConcluirAsync(int id)
    {
        var agendamento = await _agendamentoRepository.GetByIdAsync(id);
        if (agendamento == null)
            throw new KeyNotFoundException("Agendamento não encontrado");

        // Só pode concluir se estiver Confirmado
        if (agendamento.Status != StatusAgendamento.Confirmado)
            throw new InvalidOperationException(
                $"Não é possível concluir um agendamento com status '{agendamento.Status}'");

        agendamento.Status = StatusAgendamento.Concluido;
        await _agendamentoRepository.UpdateAsync(agendamento);
        return MapToResponse(agendamento);
    }

    public async Task<AgendamentoResponseDTO> CancelarAsync(int id, AgendamentoCancelarDTO dto)
    {
        var agendamento = await _agendamentoRepository.GetByIdAsync(id);
        if (agendamento == null)
            throw new KeyNotFoundException("Agendamento não encontrado");

        // Não pode cancelar o que já foi concluído ou já cancelado
        if (agendamento.Status == StatusAgendamento.Concluido)
            throw new InvalidOperationException("Não é possível cancelar um agendamento já concluído");

        if (agendamento.Status == StatusAgendamento.Cancelado)
            throw new InvalidOperationException("Este agendamento já está cancelado");

        // Verifica antecedência mínima
        var horasRestantes = (agendamento.DataHora - DateTime.Now).TotalHours;
        if (horasRestantes < AntecedenciaMinimaHoras)
            throw new InvalidOperationException(
                $"O cancelamento deve ser feito com no mínimo {AntecedenciaMinimaHoras} horas de antecedência");

        agendamento.Status = StatusAgendamento.Cancelado;
        agendamento.Observacoes = $"Cancelado: {dto.Motivo}";
        await _agendamentoRepository.UpdateAsync(agendamento);
        return MapToResponse(agendamento);
    }

    public async Task<ResumoDiaDTO> GetResumoDiaAsync(DateTime data)
    {
        var agendamentos = await _agendamentoRepository.GetResumoDiaAsync(data);

        return new ResumoDiaDTO
        {
            Data = data.Date,
            TotalAgendamentos = agendamentos.Count,
            Agendados = agendamentos.Count(a => a.Status == StatusAgendamento.Agendado),
            Confirmados = agendamentos.Count(a => a.Status == StatusAgendamento.Confirmado),
            Concluidos = agendamentos.Count(a => a.Status == StatusAgendamento.Concluido),
            Cancelados = agendamentos.Count(a => a.Status == StatusAgendamento.Cancelado),
            Agendamentos = agendamentos.Select(MapToResponse).ToList()
        };
    }

    private static AgendamentoResponseDTO MapToResponse(Agendamento a) => new()
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