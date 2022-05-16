using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PCO.Data;
using PCO.Models;
using PCO.Models.Interfaces;
using PCO.Models.PlaceModels;
using PCO.Models.UserModels;

namespace PCO.Controllers
{
    public class PlaceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _appEnvironment;
        private readonly IRepository<Place> PlaceRepository;
        //For tests
        /*public PlaceController(IRepository<Place> repository)
        {
            PlaceRepository = repository;
            _context = new ApplicationDbContext();
        }*/
        public PlaceController(ApplicationDbContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            PlaceRepository = new PlaceRepository(_context);
        }
        public IActionResult Index(string placeCategory, string searchString)
        {
            return View(CreatePlaceCategoryViewModel(placeCategory, searchString));
        }
        public IActionResult List(string placeCategory, string searchString)
        {
            return View(CreatePlaceCategoryViewModel(placeCategory, searchString));
        }
        public async Task<IActionResult> PlaceDetails(int? id, string Comment, IFormFile uploadedFile)
        {
            var placeModel = await GetPlaceAsync(id);

            if (placeModel == null)
            {
                return NotFound();
            }
            var placeDetailsVM = new PlaceDetailsVM()
            {
                Place = placeModel,
            };
            if (!string.IsNullOrEmpty(Comment))
            {
                placeDetailsVM.Comments.Add(CreateComment(placeModel, Comment));
            }
            if (uploadedFile != null)
            {
                placeDetailsVM.Files.Add(CreateFile(placeModel, uploadedFile));
            }
            if (placeModel.Media.Any())
            {
                placeDetailsVM.Files = placeModel.Media;
            }
            if (placeModel.Comments.Any())
            {
                placeDetailsVM.Comments = placeModel.Comments;
            }
            return View(placeDetailsVM);
        }
        private FileModel CreateFile(Place place, IFormFile uploadedFile)
        {
            string path = "/Files/" + uploadedFile.FileName;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                uploadedFile.CopyTo(fileStream);
            }
            FileModel file = new() 
            { 
                Name = uploadedFile.FileName, 
                Path = path, PlaceWhereAttached = place, 
                UserWhoAttached = User.Identity.Name
            };
            if( _context.Files.FirstOrDefaultAsync(f => f.Name == path) == null)
            {
                _context.Add(file);
            }
            _context.SaveChangesAsync();

            return file;
        }
        private Comment CreateComment(Place place, string comment)
        {
            var com = new Comment()
            {
                Content = comment,
                Created = DateTime.Now,
                PlaceWhereLeft = place,
                UserWhoLeft = User.Identity.Name,
            };
            _context.Add(com);
            _context.SaveChangesAsync();
            return com;
        }
        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpGet]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public IActionResult Create([Bind("Id,Name,Category,Country,City")] Place placeModel)
        {
            if (ModelState.IsValid)
            {
                _context.Requests.Add(new RequestStore() { UserWhoAddedRequest = User.Identity.Name, IsCreated = true, Place = placeModel});
                //ForTests
                //PlaceRepository.Create(placeModel);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(placeModel);
        }
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Details(int? id)
        {
            var placeModel = await GetPlaceAsync(id);
            if (placeModel == null)
            {
                return NotFound();
            }

            return View(placeModel);
        }
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Places == null)
            {
                return NotFound();
            }

            var placeModel = await _context.Places.FindAsync(id);
            if (placeModel == null)
            {
                return NotFound();
            }
            return View(placeModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Category,Country,City")] Place placeModel)
        {
            if (id != placeModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var requestedPlace = await _context.Places.FindAsync(id);
                    var editedPlace = new RequestedPlace()
                    {
                        Name = placeModel.Name,
                        Category = placeModel.Category,
                        Country = placeModel.Country,
                        City = placeModel.City,
                        Comments = placeModel.Comments,
                        Media = placeModel.Media,
                        UsersWhoVisited = placeModel.UsersWhoVisited,
                    };
                    _context.Requests.Add(new RequestStore()
                    {
                        UserWhoAddedRequest = User.Identity.Name,
                        IsEdited = true,
                        Place = requestedPlace,
                        RequestedPlace = editedPlace
                    });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaceModelExists(placeModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(placeModel);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Places == null)
            {
                return NotFound();
            }

            var placeModel = await _context.Places
                .FirstOrDefaultAsync(m => m.Id == id);
            if (placeModel == null)
            {
                return NotFound();
            }

            return View(placeModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var placeModel = await GetPlaceAsync(id);
            if (placeModel == null)
            {
                return NotFound();
            }
            _context.Add(new RequestStore{ UserWhoAddedRequest = User.Identity.Name, Place = placeModel, IsDeleted = true });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaceModelExists(int? id)
        {
          return (_context.Places?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private async Task<Place> GetPlaceAsync(int? id)
        {
            if(id == null || _context.Places == null)
            {
                return null;
            }
            var placeModel = await _context.Places.FirstOrDefaultAsync(e => e.Id == id);
            if (placeModel==null)
            {
                return null;
            }
            return placeModel;
        }
        private PlaceCategoryViewModel CreatePlaceCategoryViewModel(string placeCategory, string searchString)
        {
            IQueryable<string> categoryQuery = from m in _context.Places
                                               orderby m.Category
                                               select m.Category;
            var places = from m in _context.Places
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                places = places.Where(s => s.Name!.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!string.IsNullOrEmpty(placeCategory))
            {
                places = places.Where(x => x.Category == placeCategory);
            }

            var placeCategoryVM = new PlaceCategoryViewModel
            {
                Categories = new SelectList(categoryQuery.Distinct().ToList()),
                Places = places.ToList()
            };
            return placeCategoryVM;
        }
        public IActionResult GetPlace(int? id)
        {
            if (!id.HasValue)
                return BadRequest();
            Place place = PlaceRepository.Get(id.Value);
            if (place == null)
                return NotFound();
            return View(place);
        }
    }
}
