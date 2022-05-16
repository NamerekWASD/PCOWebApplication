using Microsoft.EntityFrameworkCore;
using PCO.Data;
using PCO.Models.Interfaces;
using PCO.Models.PlaceModels;

namespace PCO.Models
{
    public class PlaceRepository : IRepository<Place>
    {
        private readonly ApplicationDbContext _context;
        public PlaceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Create(Place place)
        {
            _context.Places.Add(place);
            _context.SaveChanges();
        }

        public void Delete(int? id)
        {
            var place = _context.Places.Find(id);
            _context.Places.Remove(place);
            _context.SaveChanges();
        }

        public List<Place> GetAll()
        {
            return _context.Places.ToList();
        }

        public Place Get(int? id)
        {
            return _context.Places.Single(p => p.Id == id);
        }

        public void Update(Place place)
        {
            _context.Entry(place).State = EntityState.Modified;
        }
    }
}
