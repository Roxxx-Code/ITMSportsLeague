namespace SportsLeague.API.DTOs.Request;

public class SponsorRequestDTO
{
    public string Name { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty; // Puede ser null
    public string? WebsiteUrl { get; set; } = string.Empty;
    public SponsorCategory Category { get; set; }
}
