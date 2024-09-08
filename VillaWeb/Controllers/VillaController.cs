using Microsoft.AspNetCore.Mvc;
using VillaBooking.Infrastructure.Data;

namespace VillaBooking.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaController(ApplicationDbContext context)
        {
            _db = context;
        }
        public IActionResult Index()
        {
            var villas = _db.Villas.ToList();
            return View(villas);
        }
    }
}
