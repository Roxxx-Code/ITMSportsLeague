namespace SportsLeague.API.DTOs.Request;

public class TournamentSponsorRequestDTO
{
    public decimal ContractAmount { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}
