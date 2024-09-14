using Microsoft.AspNetCore.Mvc;
using VillaBooking.Application.Common.Interfaces;
using VillaBooking.Domain.Entities;
using VillaBooking.Infrastructure.Data;

namespace VillaBooking.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll();
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
                _unitOfWork.Villa.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "The Villa has been created successfully.";

                return RedirectToAction("Index");
            }

            return View(obj);
        }

        public IActionResult Update(int villaId) 
        {
            Villa? obj = _unitOfWork.Villa.Get(v => v.Id == villaId);

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
                _unitOfWork.Villa.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "The Villa has been updated successfully.";

                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int villaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(v => v.Id == villaId);

            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }


        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _unitOfWork.Villa.Get(v => v.Id == obj.Id);
            if (objFromDb is not null)
            {
                _unitOfWork.Villa.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The Villa has been deleted successfully.";

                return RedirectToAction("Index");
            }

            TempData["error"] = "The Villa could not be deleted.";

            return View();
        }
    }
}
