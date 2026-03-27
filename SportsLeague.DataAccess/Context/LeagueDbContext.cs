using Microsoft.EntityFrameworkCore;
using SportsLeague.Domain.Entities;
namespace SportsLeague.DataAccess.Context;


public class LeagueDbContext : DbContext

{
    public LeagueDbContext(DbContextOptions<LeagueDbContext> options)

    : base(options)

    {

    }


    public DbSet<Team> Teams => Set<Team>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)

    {

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Team>(entity =>

        {

            entity.HasKey(t => t.Id);

            entity.Property(t => t.Name)

            .IsRequired()

            .HasMaxLength(100);

            entity.Property(t => t.City)

            .IsRequired()

            .HasMaxLength(100);

            entity.Property(t => t.Stadium)

            .HasMaxLength(150);

            entity.Property(t => t.LogoUrl)

            .HasMaxLength(500);

            entity.Property(t => t.CreatedAt)

            .IsRequired();

            entity.Property(t => t.UpdatedAt)

            .IsRequired(false);

            entity.HasIndex(t => t.Name)

            .IsUnique();

        });

    }

}

// La clase LeagueDbContext es el contexto de base de datos que hereda de DbContext.
// Define un DbSet<Team> para acceder a la tabla de equipos en la base de datos.
// En el método OnModelCreating, se configuran las propiedades de la entidad Team, como claves primarias,
// restricciones de longitud, índices y requisitos de campos.