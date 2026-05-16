using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Services;

public interface IMatchLineupService
{
    Task<MatchLineup> RegisterLineupAsync(int matchId, MatchLineup lineup);
    Task<IEnumerable<MatchLineup>> GetLineupsByMatchAsync(int matchId);
    Task DeleteLineupAsync(int lineupId);
}
