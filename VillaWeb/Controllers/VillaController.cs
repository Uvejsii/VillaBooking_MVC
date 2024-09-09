using Microsoft.AspNetCore.Mvc;
using VillaBooking.Domain.Entities;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa obj) 
        {
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("", "The descripton cannot exactly match the name");
            }
            if (ModelState.IsValid)
            {
                _db.Villas.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "The Villa has been created successfully.";

                return RedirectToAction("Index");
            }

            return View(obj);
        }

        public IActionResult Update(int villaId) 
        {
            Villa? obj = _db.Villas.Find(villaId);

            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }


        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if (ModelState.IsValid && obj.Id > 0)
            {
                _db.Villas.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "The Villa has been updated successfully.";

                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int villaId)
        {
            Villa? obj = _db.Villas.Find(villaId);

            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }


        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _db.Villas.FirstOrDefault(v => v.Id == obj.Id);
            if (objFromDb is not null)
            {
                _db.Villas.Remove(objFromDb);
                _db.SaveChanges();
                TempData["success"] = "The Villa has been deleted successfully.";

                return RedirectToAction("Index");
            }

            TempData["error"] = "The Villa could not be deleted.";

            return View();
        }
    }
}
