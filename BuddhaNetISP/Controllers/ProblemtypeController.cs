using BuddhaNetISP.DTO;
using BuddhaNetISP.Implementation;
using BuddhaNetISP.Interface;
using BuddhaNetISP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BuddhaNetISP.Controllers
{

    [Authorize]
    public class ProblemtypeController : Controller
    {
        private readonly IProblemtypeRepo _problemtypeRepo;
        public ProblemtypeController(IProblemtypeRepo problemtypeRepo)
        {
            _problemtypeRepo = problemtypeRepo; 
        }
        public IActionResult Create() 
        {
          return View();    
        }
        [HttpPost]
        public IActionResult Create(ProblemtypeDTO problemtypeDTO)
        {
            if (ModelState.IsValid)
            {
                var response = _problemtypeRepo.SaveProblemType(problemtypeDTO);
                if (response.IsSuccess)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                }
            }
            return View(problemtypeDTO);
        }
        public IActionResult List()
        {
            var problemTypeList = _problemtypeRepo.GetAllProblemTypes();
            if (problemTypeList.IsSuccess)
            {
                List<Problemtype> finalList = problemTypeList.ResponseData as List<Problemtype>;
                return View(finalList);
            }
            else
            {
                return View(new List<ProblemtypeDTO>());
            }
        }
        public IActionResult Update(int id)
        {
            var dataFromId = _problemtypeRepo.GetProblemTypeById(id);
            var toUpdateData = dataFromId.ResponseData as Problemtype;
            return View(toUpdateData);
        }

        [HttpPost]
        public IActionResult Update(int id, ProblemtypeDTO problemtypeDTO)
        {
            if (ModelState.IsValid)
            {
                var response = _problemtypeRepo.UpdateProblemType(problemtypeDTO);
                if (response.IsSuccess)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                }
            }
            return View(problemtypeDTO);
        }
        public IActionResult Delete(int problemTypeId)
        {
            _problemtypeRepo.DeleteProblemType(problemTypeId);
            return RedirectToAction("List");
        }
    }
}
