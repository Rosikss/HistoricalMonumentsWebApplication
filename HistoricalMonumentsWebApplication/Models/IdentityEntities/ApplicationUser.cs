using Microsoft.AspNetCore.Identity;

namespace HistoricalMonumentsWebApplication.Models.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? PersonName { get; set; }
        public List<int>? FavouriteHistoricalMonuments { get; set; }
    }
}
