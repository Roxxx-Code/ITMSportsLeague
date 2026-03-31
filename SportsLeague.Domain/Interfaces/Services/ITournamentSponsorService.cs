using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Interfaces.Services;

public interface ITournamentSponsorService
{
    Task LinkSponsorAsync(int tournamentId, int sponsorId, decimal contractAmount);
    Task UnlinkSponsorAsync(int tournamentId, int sponsorId);
    Task<IEnumerable<TournamentSponsor>> GetSponsorsByTournamentAsync(int tournamentId);
    Task<IEnumerable<TournamentSponsor>> GetTournamentsBySponsorAsync(int sponsorId);
    
}