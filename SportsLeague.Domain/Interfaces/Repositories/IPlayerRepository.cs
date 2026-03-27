using SportsLeague.Domain.Entities;
namespace SportsLeague.Domain.Interfaces.Repositories;

public interface IPlayerRepository : IGenericRepository<Player>

{
    Task<IEnumerable<Player>> GetByTeamAsync(int teamId);
    Task<Player?> GetByTeamAndNumberAsync(int teamId, int number);
    Task<IEnumerable<Player>> GetAllWithTeamAsync();
    Task<Player?> GetByIdWithTeamAsync(int id);

}

// La interfaz IPlayerRepository extiende de IGenericRepository<Player>,
// lo que significa que hereda métodos genéricos para operaciones CRUD básicas (Create, Read, Update, Delete) para la entidad Player.
// Además, define métodos específicos para obtener jugadores por equipo, por número dentro de un equipo, y para incluir la información del equipo al obtener jugadores.