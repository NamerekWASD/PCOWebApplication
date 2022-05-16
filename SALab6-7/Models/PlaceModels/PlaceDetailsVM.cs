using PCO.Models.UserModels;

namespace PCO.Models.PlaceModels
{
    public class PlaceDetailsVM
    {
        public Place Place { get; set; }
        public List<FileModel>? Files { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<User>? UserProfiles { get; set; }
        public IFormFile UploadedFile { get; set; }
        public string Comment { get; set; }
    }
}
