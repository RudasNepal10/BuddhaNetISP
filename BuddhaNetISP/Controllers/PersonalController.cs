using BuddhaNetISP.DTO;
using BuddhaNetISP.Interface;
using BuddhaNetISP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuddhaNetISP.Controllers
{

    [Authorize]
    public class PersonalController : Controller
    {
        private readonly IPersonalRepo _personalRepo;
        public PersonalController(IPersonalRepo personalRepo)
        {
            _personalRepo = personalRepo;   
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(PersonalDTO personalDTO)
        {
            if (ModelState.IsValid)
            {
                var response = _personalRepo.SavePersonal(personalDTO);
                if (response.IsSuccess)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                }
            }
            return View(personalDTO);
        }
        public IActionResult List()
        {
            var personalList = _personalRepo.GetAllPersonals();
            if (personalList.IsSuccess)
            {
                List<Personal> finalList = personalList.ResponseData as List<Personal>;
                return View(finalList);
            }
            else
            {
                return View(new List<PersonalDTO>());
            }
        }
        public IActionResult Update(int id)
        {
            var dataFromId = _personalRepo.GetPersonalById(id);
            var toUpdateData = dataFromId.ResponseData as Personal;
            return View(toUpdateData);
        }

        [HttpPost]
        public IActionResult Update(int id, PersonalDTO personalDTO)
        {
            if (ModelState.IsValid)
            {
                var response = _personalRepo.UpdatePersonal(personalDTO);
                if (response.IsSuccess)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                }
            }
            return View(personalDTO);
        }
        public IActionResult Delete(int personnelid) 
        { 
         _personalRepo.DeletePersonal(personnelid); 
          return RedirectToAction("List");
        
        }

    }
}
