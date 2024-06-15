using BuddhaNetISP.DTO;
using BuddhaNetISP.Implementation;
using BuddhaNetISP.Interface;
using BuddhaNetISP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Editing;

namespace BuddhaNetISP.Controllers
{

    [Authorize]
    public class SpecialistController : Controller
    {
        private readonly IspecialistRepo _specialistrepo;
        private readonly IProblemtypeRepo _problemtypeRepo;
        public SpecialistController(IspecialistRepo specialistrepo, IProblemtypeRepo problemtypeRepo)
        {
            _specialistrepo = specialistrepo;
            _problemtypeRepo = problemtypeRepo;
        }

        public IActionResult Create()
        {
            var response = _problemtypeRepo.GetAllProblemTypes();
            if (response.IsSuccess)
            {
                List<Problemtype> helpdeskoperatorlist = response.ResponseData as List<Problemtype>;
                ViewBag.helpdeskoperatorslist = new SelectList(helpdeskoperatorlist, "problemtypeid", "typename");
            }
            else
            {
                ViewBag.helpdeskoperatorslist = new SelectList(new List<Problemtype>());
                ModelState.AddModelError(string.Empty, response.Message);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Create(SpecialistDTO specialistDTO)
        {
            if (ModelState.IsValid)
            {
                var response = _specialistrepo.SaveSpecialist(specialistDTO);
                if (response.IsSuccess)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                }
            }
            return View(specialistDTO);
        }
        public IActionResult List()
        {
            var specialistList = _specialistrepo.GetAllSpecialists();
            if (specialistList.IsSuccess)
            {
                List<Specialist> finalList = specialistList.ResponseData as List<Specialist>;
                return View(finalList);
            }
            else
            {
                return View(new List<SpecialistDTO>());
            }
        }
        public IActionResult Update(int id)
        {
            var response = _specialistrepo.GetSpecialistById(id);
            if (response.IsSuccess)
            {
                var toUpdateData = response.ResponseData as Specialist;

                var problemTypeResponse = _problemtypeRepo.GetAllProblemTypes();
                if (problemTypeResponse.IsSuccess)
                {
                    List<Problemtype> helpdeskoperatorlist = problemTypeResponse.ResponseData as List<Problemtype>;
                    ViewBag.helpdeskoperatorslist = new SelectList(helpdeskoperatorlist, "problemtypeid", "typename");
                }
                else
                {
                    ViewBag.helpdeskoperatorslist = new SelectList(new List<Problemtype>());
                    ModelState.AddModelError(string.Empty, problemTypeResponse.Message);
                }

                return View(toUpdateData);
            }
            else
            {
                ModelState.AddModelError(string.Empty, response.Message);
                return RedirectToAction("List"); 
            }
        }
         
        [HttpPost]
        public IActionResult Update(int id, SpecialistDTO specialistDTO)
        {
            if (ModelState.IsValid)
            {
                var response = _specialistrepo.UpdateSpecialist(specialistDTO);
                if (response.IsSuccess)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                }
            }
            return View(specialistDTO);
        }
        public ActionResult Delete(int specialistid)
        {
            _specialistrepo.DeleteSpecialist(specialistid);
            return RedirectToAction("list");

        }
    }
}
