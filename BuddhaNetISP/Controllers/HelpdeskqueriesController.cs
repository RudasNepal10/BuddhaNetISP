using BuddhaNetISP.DTO;
using BuddhaNetISP.Interface;
using BuddhaNetISP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BuddhaNetISP.Controllers
{

    [Authorize]
    public class HelpdeskqueriesController : Controller
    {
        private readonly IHelpdeskqueriesRepo _helpdeskqueriesRepo;
        private readonly IPersonalRepo _personalRepo;
        private readonly IEquipmenrRepo _equipmenrRepo;
        private readonly IHelpdeskoperatorRepo _helpdeskoperatorRepo;
        private readonly IProblemtypeRepo _problemtypeRepo;
        private readonly ISoftwareRepo _softwareRepo;
        private readonly IspecialistRepo _specialistRepo;
        public HelpdeskqueriesController(IHelpdeskqueriesRepo helpdeskqueriesRepo, IPersonalRepo personalRepo, IEquipmenrRepo equipmenrRepo, IHelpdeskoperatorRepo helpdeskoperatorRepo, IProblemtypeRepo problemtypeRepo, ISoftwareRepo softwareRepo, IspecialistRepo specialistRepo )
        {
            _helpdeskqueriesRepo = helpdeskqueriesRepo; 
            _personalRepo = personalRepo;   
            _equipmenrRepo = equipmenrRepo;
            _helpdeskoperatorRepo = helpdeskoperatorRepo;
            _problemtypeRepo = problemtypeRepo;
            _softwareRepo = softwareRepo;
            _specialistRepo = specialistRepo;
        }
        public IActionResult Create()
        {
            var personalResponse=_personalRepo.GetAllPersonals();
            var EquipmentResponse=_equipmenrRepo.GetAllEquipments();
            var problemTypesResponse = _problemtypeRepo.GetAllProblemTypes();
            var operatorsResponse = _helpdeskoperatorRepo.GetAllHelpDeskOperators();
            var softwareresponse= _softwareRepo.GetAllSoftware();
            var specialistResponse=_specialistRepo.GetAllSpecialists();

            if (problemTypesResponse.IsSuccess &&operatorsResponse.IsSuccess && personalResponse.IsSuccess&& EquipmentResponse.IsSuccess&& softwareresponse.IsSuccess&& specialistResponse.IsSuccess)
            {
                List<Problemtype> problemTypes = problemTypesResponse.ResponseData as List<Problemtype>;
                List<Helpdeskoperator> operators = operatorsResponse.ResponseData as List<Helpdeskoperator>;
                List<Software> softwares = softwareresponse.ResponseData as List<Software>;
                List<Specialist> specialist = specialistResponse.ResponseData as List<Specialist>;
                List<Personal> personals = personalResponse.ResponseData as List<Personal>;
                List<EquipementModel> equipements= EquipmentResponse.ResponseData as List<EquipementModel>;    

                ViewBag.ProblemTypes = new SelectList(problemTypes, "problemtypeid", "typename");
                ViewBag.Operators = new SelectList(operators, "operatorid", "name");
                ViewBag.Software = new SelectList(softwares, "softwareid", "softwarename");
                ViewBag.Specialist = new SelectList(specialist, "specialistid", "specialistname");
                ViewBag.equipment = new SelectList(equipements, "equipmentid", "equipmenttype");
                ViewBag.Personal = new SelectList(personals, "personnelid", "name");
            }
            else
            {
                ViewBag.ProblemTypes = new SelectList(new List<Problemtype>());
                ViewBag.Operators = new SelectList(new List<Helpdeskoperator>());
                ViewBag.Software= new SelectList(new List<Software>()); 
                ViewBag.Specialist= new SelectList(new List<Specialist>());
                ViewBag.Personal= new SelectList(new List<Personal>()); 
                ViewBag.EquipementModel = new SelectList(new List<EquipementModel>()); 
                ModelState.AddModelError(string.Empty, "Failed to fetch dropdown data.");
            }

            return View();
        }
        [HttpPost]
        public IActionResult Create(HelpdeskqueriesDTO helpdeskQueryDTO)
        {
            if (ModelState.IsValid)
            {
                var response = _helpdeskqueriesRepo.SaveHelpdeskQuery(helpdeskQueryDTO);
                if (response.IsSuccess)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                }
            }
            return View(helpdeskQueryDTO);
        }
        public IActionResult List()
        {
            var queryList = _helpdeskqueriesRepo.GetAllHelpdeskQueries();
            if (queryList.IsSuccess)
            {
                List<Helpdeskqueries> finalList = queryList.ResponseData as List<Helpdeskqueries>;
                return View(finalList);
            }
            else
            {
                return View(new List<HelpdeskqueriesDTO>());
            }
        }
        public IActionResult Delete(int queryId) 
        {
            _helpdeskqueriesRepo.DeleteHelpdeskQuery(queryId);
            return RedirectToAction("List");
        }

    }
}
