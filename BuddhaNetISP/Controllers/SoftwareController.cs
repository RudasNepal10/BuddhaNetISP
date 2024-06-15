using BuddhaNetISP.DTO;
using BuddhaNetISP.Interface;
using BuddhaNetISP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuddhaNetISP.Controllers
{

    [Authorize]
    public class SoftwareController : Controller
    {
        private readonly ISoftwareRepo _softwareRepo;
        public SoftwareController(ISoftwareRepo softwareRepo)
        {
            _softwareRepo = softwareRepo;   
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(SoftwareDTO softwareDTO)
        {
            if (ModelState.IsValid)
            {
                var response = _softwareRepo.SaveSoftware(softwareDTO);
                if (response.IsSuccess)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                }
            }
            return View(softwareDTO);
        }
        public IActionResult List()
        {
            var softwareList = _softwareRepo.GetAllSoftware();
            if (softwareList.IsSuccess)
            {
                List<Software> finalList = softwareList.ResponseData as List<Software>;
                foreach (var item in finalList)
                {
                    item.LiscenceStatus = item.isunderlicense ? "Has Liscence" : "No Liscence";
                    
                }
                return View(finalList);
            }
            else
            {
                return View(new List<SoftwareDTO>());
            }
        }
        public IActionResult Update(int id)
        {
            var dataFromId = _softwareRepo.GetSoftwareById(id);
            var toUpdateData = dataFromId.ResponseData as Software;
            return View(toUpdateData);
        }

        [HttpPost]
        public IActionResult Update(int id, SoftwareDTO softwareDTO)
        {
            if (ModelState.IsValid)
            {
                var response = _softwareRepo.UpdateSoftware(softwareDTO);
                if (response.IsSuccess)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                }
            }
            return View(softwareDTO);
        }
        public IActionResult Delete(int softwareid) 
        { 
         _softwareRepo.DeleteSoftware(softwareid);
            return RedirectToAction("List");
        
        }


    }
}
