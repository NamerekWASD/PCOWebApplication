using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PCO.Controllers
{
    [Authorize(Policy = "Manager")]
    public class ManagerController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
