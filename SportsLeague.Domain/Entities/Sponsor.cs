
namespace SportsLeague.Domain.Entities
{
    public class Sponsor : AuditBase
    {
        //No se requiere declarar la propiedad Id porque esta en el AuditBase

        public string Name { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty; // Puede ser null
        public string? WebsiteUrl { get; set; } = string.Empty;
        public SponsorCategory Category { get; set; }

        //no requiere createdAt ni el updatedAt porque estas propiedades ya están en el AuditBase

        // Navigation Properties

        public ICollection<Sponsor> Sponsors { get; set; } = new List<Sponsor>();
        public ICollection<TournamentSponsor> TournamentSponsors { get; set; } = new List<TournamentSponsor>();
    }
}
