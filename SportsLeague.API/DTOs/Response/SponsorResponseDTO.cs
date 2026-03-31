namespace SportsLeague.API.DTOs.Response;

public class SponsorResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty; // Puede ser null
    public string? WebsiteUrl { get; set; } = string.Empty;
    public SponsorCategory Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
