using BuddhaNetISP.DTO;
using BuddhaNetISP.Implementation;
using BuddhaNetISP.Interface;
using BuddhaNetISP.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BuddhaNetISP.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly IEquipmenrRepo _equipmenrRepo;
        public EquipmentController(IEquipmenrRepo equipmenrRepo)
        {
            _equipmenrRepo = equipmenrRepo;
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(EquipmentDTO equipmentDTO)
        {
            if (ModelState.IsValid)
            {
                var response = _equipmenrRepo.SaveEquipment(equipmentDTO);
                if (response.IsSuccess)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                }
            }
            return View(equipmentDTO);
        }
        public IActionResult List()
        {
            var equipmentList = _equipmenrRepo.GetAllEquipments();
            if (equipmentList.IsSuccess)
            {
                List<EquipementModel> finalList = equipmentList.ResponseData as List<EquipementModel>;
                foreach (var item in finalList) {
                    item.LiscenceStatus = item.isunderlicense ? "Has Liscence" : "No Liscence";
                }

                return View(finalList);
            }
            else 
            {
                return View(new List<EquipmentDTO>());
            }
           
        }
        public IActionResult Update(int id)
        {
            var datafromId= _equipmenrRepo.GetEquipmentById(id);
            var toupdatedata = datafromId.ResponseData as EquipementModel;
            return View(toupdatedata);
        }
        [HttpPost]
        public IActionResult Update(int id, EquipmentDTO equipmentDTO)
        {
            if (ModelState.IsValid)
            {
                var response = _equipmenrRepo.UpdateEquipment(equipmentDTO);
                if (response.IsSuccess)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                }
            }
            return View(equipmentDTO);
        }
        public IActionResult Delete(string serialnumber) 
        { 
         _equipmenrRepo.DeleteEquipment(serialnumber);
           return RedirectToAction("List");
        }
    }
}

