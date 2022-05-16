using Microsoft.AspNetCore.Identity;
using PCO.Models.PlaceModels;

namespace PCO.Models.UserModels
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public virtual List<Place>? VisitedPlaces { get; set; }
    }
}
