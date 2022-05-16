namespace PCO.Models.PlaceModels
{
    public class FileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string UserWhoAttached { get; set; }
        public virtual Place PlaceWhereAttached { get; set; }
    }
}
