namespace SportsLeague.Domain.Entities;

public class Team : AuditBase

{

    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Stadium { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public DateTime FoundedDate { get; set; }


    // Navigation Properties

    public ICollection<Player> Players { get; set; } = new List<Player>();
    public ICollection<TournamentTeam> TournamentTeams { get; set; } = new List<TournamentTeam>();

    // Partidos como local
    public ICollection<Match> HomeMatches { get; set; } = new List<Match>();

    // Partidos como visitante
    public ICollection<Match> AwayMatches { get; set; } = new List<Match>();
}

// La clase Team representa un equipo deportivo en la liga. Hereda de AuditBase para incluir campos comunes como Id, CreatedAt y UpdatedAt.
// Contiene propiedades específicas del equipo, como Name, City, Stadium, LogoUrl y FoundedDate.