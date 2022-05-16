using PCO.Models.UserModels;

namespace PCO.Models.PlaceModels
{
    public class Comment
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string UserWhoLeft { get; set; }
        public int PlaceWhereLeftId { get; set; }
        public virtual Place PlaceWhereLeft { get; set; }
        public string Content { get; set; }
    }
}
