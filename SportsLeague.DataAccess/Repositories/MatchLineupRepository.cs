using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories;

public class MatchLineupRepository : GenericRepository<MatchLineup>, IMatchLineupRepository
{
    public MatchLineupRepository(LeagueDbContext context) : base(context) { }

    public async Task<MatchLineup?> GetByMatchAndPlayerAsync(int matchId, int playerId)
    {
        return await _dbSet
            .Where(ml => ml.MatchId == matchId && ml.PlayerId == playerId)
            .Include(ml => ml.Player)
                .ThenInclude(p => p.Team)
            .FirstOrDefaultAsync();
    }

    public async Task<int> GetStartersCountByMatchAndTeamAsync(int matchId, int teamId)
    {
        return await _dbSet
            .Include(ml => ml.Player)
            .Where(ml => ml.MatchId == matchId && ml.IsStarter && ml.Player.TeamId == teamId)
            .CountAsync();
    }

    public async Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId)
    {
        return await _dbSet
            .Where(ml => ml.MatchId == matchId)
            .Include(ml => ml.Player)
                .ThenInclude(p => p.Team)
            .ToListAsync();
    }
}
