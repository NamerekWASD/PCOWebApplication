using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCO.Models.PlaceModels
{
    [NotMapped]
    public class PlaceCategoryViewModel
    {
        public List<Place>? Places { get; set; }
        public SelectList? Categories { get; set; }
        public string? PlaceCategory { get; set; }
        public string? SearchString { get; set; }
    }
}
