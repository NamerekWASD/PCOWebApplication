using PCO.Models.UserModels;
using System.ComponentModel.DataAnnotations;

namespace PCO.Models.PlaceModels
{
    public class Place
    {
        public int Id { get; set; }
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Category { get; set; }
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Country { get; set; }
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string City { get; set; }
        public virtual List<User>? UsersWhoVisited { get; set; } = new List<User>();
        public virtual List<Comment>? Comments { get; set; } = new List<Comment>();
        public virtual List<FileModel>? Media { get; set; } = new List<FileModel>();
    }
}
