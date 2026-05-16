using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories;

public interface IMatchLineupRepository : IGenericRepository<MatchLineup>
{
    Task<MatchLineup?> GetByMatchAndPlayerAsync(int matchId, int playerId);
    Task<int> GetStartersCountByMatchAndTeamAsync(int matchId, int teamId);
    Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId);
}
