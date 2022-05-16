using PCO.Models.UserModels;

namespace PCO.Models.PlaceModels
{
    public class RequestedPlace
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public virtual List<User>? UsersWhoVisited { get; set; } = new List<User>();
        public virtual List<Comment>? Comments { get; set; } = new List<Comment>();
        public virtual List<FileModel>? Media { get; set; } = new List<FileModel>();
    }
}
