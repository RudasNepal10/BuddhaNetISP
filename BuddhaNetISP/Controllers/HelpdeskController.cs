using BuddhaNetISP.DTO;
using BuddhaNetISP.Implementation;
using BuddhaNetISP.Interface;
using BuddhaNetISP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuddhaNetISP.Controllers
{

    [Authorize]
    public class HelpdeskController : Controller
    {
        private readonly IHelpdeskoperatorRepo _helpdeskoperatorRepo;
        public HelpdeskController(IHelpdeskoperatorRepo helpdeskoperatorRepo)
        {
            _helpdeskoperatorRepo = helpdeskoperatorRepo;   
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(HelpdeskoperatorDTO helpdeskoperatorDTO)
        {
            if (ModelState.IsValid)
            {
                var response = _helpdeskoperatorRepo.SaveHelpDeskOperator(helpdeskoperatorDTO);
                if (response.IsSuccess)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                }
            }
            return View(helpdeskoperatorDTO);
        }
        public IActionResult List()
        {
            var operatorList = _helpdeskoperatorRepo.GetAllHelpDeskOperators();
            if (operatorList.IsSuccess)
            {
               List<Helpdeskoperator> finalist = operatorList.ResponseData as List<Helpdeskoperator>;
               return View(finalist);
            }
            else
            {
                return View(new List<HelpdeskoperatorDTO>());
            }
        }
        public IActionResult Update(int id) 
        {
         var datafromId= _helpdeskoperatorRepo.GetHelpDeskOperatorById(id);
         var toupdatedata = datafromId.ResponseData as Helpdeskoperator;
          return View(toupdatedata);

        }
        [HttpPost]
        public IActionResult Update(int id, HelpdeskoperatorDTO helpdeskoperatorDTO)
        {
            if (ModelState.IsValid)
            {
                var response = _helpdeskoperatorRepo.UpdateHelpDeskOperator(helpdeskoperatorDTO);
                if (response.IsSuccess)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                }
            }
            return View(helpdeskoperatorDTO);
        }
        public IActionResult Delete(int operatorid)
        {
            _helpdeskoperatorRepo.DeleteHelpDeskOperator(operatorid);
            return RedirectToAction("List");

        }

    }
}


