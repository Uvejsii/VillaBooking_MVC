using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VillaBooking.Application.Common.Interfaces;
using VillaBooking.Domain.Entities;
using VillaBooking.Infrastructure.Data;
using VillaBooking.Web.ViewModels;

namespace VillaBooking.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var villas = _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
            return View(villas);
        }

        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString(),
                })
            };

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM obj) 
        {
            bool roomNumberExists = _unitOfWork.VillaNumber.Any(v => v.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExists)
            {
                _unitOfWork.VillaNumber.Add(obj.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The Villa Number has been created successfully.";

                return RedirectToAction("Index");
            }

            if (roomNumberExists)
            {
                TempData["error"] = "The Villa Number already exits.";
            }

            obj.VillaList = _unitOfWork.Villa.GetAll().Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString(),
            });

            return View(obj);
        }

        public IActionResult Update(int villaNumber) 
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString(),
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(v => v.Villa_Number == villaNumber)
            };

            if (villaNumberVM.VillaNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Update (VillaNumberVM villaNumberVM) 
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.VillaNumber.Update(villaNumberVM.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The Villa Number has been updated successfully.";

                return RedirectToAction("Index");
            }

            villaNumberVM.VillaList = _unitOfWork.Villa.GetAll().Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString(),
            });

            return View(villaNumberVM);
        }

        public IActionResult Delete(int villaNumber) 
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString(),
                }),

                VillaNumber = _unitOfWork.VillaNumber.Get(v => v.Villa_Number == villaNumber)
            };

            if (villaNumberVM.VillaNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? objFromDb = _unitOfWork.VillaNumber.Get(vn => vn.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);

            if (objFromDb is not null)
            {
                _unitOfWork.VillaNumber.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The Villa Number has been deleted successfully.";

                return RedirectToAction("Index");
            }

            TempData["error"] = "The Villa Number could not be deleted.";

            return View();
        }
    }
}
