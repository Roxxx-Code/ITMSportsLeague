namespace SportsLeague.API.DTOs.Request;
public class TeamRequestDTO

{

    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Stadium { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public DateTime FoundedDate { get; set; }

}

//Los DTOs (Data Transfer Objects) separan lo que exponemos al frontend de nuestras entidades internas.