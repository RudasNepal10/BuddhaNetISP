using BuddhaNetISP.DTO;

namespace BuddhaNetISP.Interface
{
    public interface IPersonalRepo
    {
        JsonResponse SavePersonal(PersonalDTO dto);
        JsonResponse UpdatePersonal(PersonalDTO dto);
        JsonResponse GetAllPersonals();
        JsonResponse DeletePersonal(int personnelid);
        JsonResponse GetPersonalById(int personnelid);
    }
}
