using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VillaBooking.Domain.Entities;
using VillaBooking.Infrastructure.Data;

namespace VillaBooking.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;
        
        public VillaNumberController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var villas = _db.VillaNumbers.ToList();
            return View(villas);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> list = _db.Villas.ToList().Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString(),
            });

            ViewBag.VillaList = list;

            return View();
        }

        [HttpPost]
        public IActionResult Create(VillaNumber obj) 
        {
            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "The Villa Number has been created successfully.";

                RedirectToAction("Index");
            }

            return View(obj);
        }

        public IActionResult Update(int villaNumber) 
        {
            VillaNumber? obj = _db.VillaNumbers.Find(villaNumber);

            if (obj == null) 
            {
                return RedirectToAction("Error", "Home");            
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Update (VillaNumber obj) 
        {
            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Update(obj);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int villaNumber) 
        {
            VillaNumber? obj = _db.VillaNumbers.Find(villaNumber);

            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }

        [HttpDelete]
        public IActionResult Delete(VillaNumber obj)
        {
            VillaNumber? objFromDb = _db.VillaNumbers.FirstOrDefault(vn => vn.Villa_Number == obj.Villa_Number);

            if (objFromDb is not null)
            {
                _db.VillaNumbers.Remove(objFromDb);
                _db.SaveChanges();
                TempData["success"] = "The Villa Number has been deleted successfully.";

                RedirectToAction("Index");
            }

            return View();
        }
    }
}
