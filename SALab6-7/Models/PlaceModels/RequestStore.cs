
namespace PCO.Models.PlaceModels
{
    public class RequestStore
    {
        public int Id { get; set; }
        public virtual Place Place { get; set; }
        public virtual RequestedPlace RequestedPlace { get; set; }
        public string UserWhoAddedRequest { get; set; }
        public bool IsCreated { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public bool IsEdited { get; set; } = false;
    }
}
