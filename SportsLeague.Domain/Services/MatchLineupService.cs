using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Helpers;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services;

public class MatchLineupService : IMatchLineupService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IMatchLineupRepository _lineupRepository;
    private readonly MatchValidationHelper _validationHelper;
    private readonly ILogger<MatchLineupService> _logger;

    public MatchLineupService(
        IMatchRepository matchRepository,
        IPlayerRepository playerRepository,
        IMatchLineupRepository lineupRepository,
        MatchValidationHelper validationHelper,
        ILogger<MatchLineupService> logger)
    {
        _matchRepository = matchRepository;
        _playerRepository = playerRepository;
        _lineupRepository = lineupRepository;
        _validationHelper = validationHelper;
        _logger = logger;
    }

    public async Task<MatchLineup> RegisterLineupAsync(int matchId, MatchLineup lineup)
    {
        // V1: Partido existe
        var match = await _matchRepository.GetByIdAsync(matchId);
        if (match == null)
            throw new KeyNotFoundException($"No se encontró el partido con ID {matchId}");

        // V6: debe estar agendado
        if (match.Status != SportsLeague.Domain.Enums.MatchStatus.Scheduled)
            throw new InvalidOperationException("Solo se pueden registrar alineaciones en partidos Scheduled");

        // V3: Player existe
        var player = await _playerRepository.GetByIdAsync(lineup.PlayerId);
        if (player == null)
            throw new KeyNotFoundException($"No se encontró el jugador con ID {lineup.PlayerId}");

        // V4: El jugador debe pertenecer al equipo local o visitante
        if (player.TeamId != match.HomeTeamId && player.TeamId != match.AwayTeamId)
            throw new InvalidOperationException("El jugador no pertenece a ninguno de los equipos del partido");

        // V4: El jugador no debe estar ya registrado en esta alineación
        var existing = await _lineupRepository.GetByMatchAndPlayerAsync(matchId, lineup.PlayerId);
        if (existing != null)
            throw new InvalidOperationException("El jugador ya está registrado en la alineación de este partido");

        // V5: Máximo 11 titulares por equipo
        if (lineup.IsStarter)
        {
            var teamId = player.TeamId;
            var startersCount = await _lineupRepository.GetStartersCountByMatchAndTeamAsync(matchId, teamId);
            if (startersCount >= 11)
                throw new InvalidOperationException("El equipo ya tiene 11 titulares registrados en este partido");
        }

        lineup.MatchId = matchId;

        _logger.LogInformation("Registering lineup: Match {MatchId}, Player {PlayerId}, Starter {IsStarter}", matchId, lineup.PlayerId, lineup.IsStarter);

        await _lineupRepository.CreateAsync(lineup);
        var created = await _lineupRepository.GetByMatchAndPlayerAsync(matchId, lineup.PlayerId);
        return created!;
    }

    public async Task<IEnumerable<MatchLineup>> GetLineupsByMatchAsync(int matchId)
    {
        var match = await _matchRepository.GetByIdAsync(matchId);
        if (match == null)
            throw new KeyNotFoundException($"No se encontró el partido con ID {matchId}");

        return await _lineupRepository.GetByMatchAsync(matchId);
    }

    public async Task DeleteLineupAsync(int lineupId)
    {
        var exists = await _lineupRepository.ExistsAsync(lineupId);
        if (!exists)
            throw new KeyNotFoundException($"No se encontró la alineación con ID {lineupId}");

        await _lineupRepository.DeleteAsync(lineupId);
    }
}
