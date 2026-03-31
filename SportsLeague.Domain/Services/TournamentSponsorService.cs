using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

public class TournamentSponsorService : ITournamentSponsorService
{
    private readonly ITournamentSponsorRepository _tsRepository;
    private readonly ISponsorRepository _sponsorRepository;
    private readonly ITournamentRepository _tournamentRepository;
    private readonly ILogger<TournamentSponsorService> _logger;



    public TournamentSponsorService(
        ITournamentSponsorRepository tsRepository,
        ISponsorRepository sponsorRepository,
        ITournamentRepository tournamentRepository,
        ILogger<TournamentSponsorService> logger)
    {
        _tsRepository = tsRepository;
        _sponsorRepository = sponsorRepository;
        _tournamentRepository = tournamentRepository;
        _logger = logger;
    }

    public async Task LinkSponsorAsync(int tournamentId, int sponsorId, decimal contractAmount)
    {
        // Validación: torneo existe
        var tournamentExists = await _tournamentRepository.ExistsAsync(tournamentId);
        if (!tournamentExists)
        {
            _logger.LogWarning("Tournament {TournamentId} not found", tournamentId);
            throw new KeyNotFoundException($"No existe el torneo con ID {tournamentId}");
        }

        // Validación: sponsor existe
        var sponsorExists = await _sponsorRepository.ExistsAsync(sponsorId);
        if (!sponsorExists)
        {
            _logger.LogWarning("Sponsor {SponsorId} not found", sponsorId);
            throw new KeyNotFoundException($"No existe el sponsor con ID {sponsorId}");
        }

        // Validación: no duplicar vínculo
        var exists = await _tsRepository.GetByTournamentAndSponsorAsync(tournamentId, sponsorId);
        if (exists != null)
        {
            _logger.LogWarning("Duplicate link Tournament {TournamentId} - Sponsor {SponsorId}", tournamentId, sponsorId);
            throw new InvalidOperationException("El sponsor ya está vinculado a este torneo");
        }

        // Validación: monto > 0
        if (contractAmount <= 0)
        {
            throw new InvalidOperationException("El monto del contrato debe ser mayor a 0");
        }

        var entity = new TournamentSponsor
        {
            TournamentId = tournamentId,
            SponsorId = sponsorId,
            ContractAmount = contractAmount,
            JoinedAt = DateTime.UtcNow
        };

        _logger.LogInformation("Linking Sponsor {SponsorId} to Tournament {TournamentId}", sponsorId, tournamentId);

        await _tsRepository.CreateAsync(entity);
    }

    public async Task UnlinkSponsorAsync(int tournamentId, int sponsorId)
    {
        var existing = await _tsRepository.GetByTournamentAndSponsorAsync(tournamentId, sponsorId);

        if (existing == null)
        {
            throw new KeyNotFoundException("La relación no existe");
        }

        _logger.LogInformation("Unlinking Sponsor {SponsorId} from Tournament {TournamentId}", sponsorId, tournamentId);

        await _tsRepository.DeleteAsync(existing.Id);
    }

    public async Task<IEnumerable<TournamentSponsor>> GetSponsorsByTournamentAsync(int tournamentId)
    {
        return await _tsRepository.GetByTournamentAsync(tournamentId);
    }

    public async Task<IEnumerable<TournamentSponsor>> GetTournamentsBySponsorAsync(int sponsorId)
    {
        return await _tsRepository.GetBySponsorAsync(sponsorId);
    }
}