using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Entities;

public class Player : AuditBase

{

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public int Number { get; set; }
    public PlayerPosition Position { get; set; }

    // Foreign Key
    public int TeamId { get; set; }

    // Navigation Property
    public Team Team { get; set; } = null!;

}

//TeamId es la Foreign Key (el valor int que se almacena en la columna de la BD).
//Team es la Navigation Property (el objeto completo que EF Core puede cargar).
//El = null! le dice al compilador que confiamos en que EF Core inicializará esta propiedad, evitando warnings de nullable.