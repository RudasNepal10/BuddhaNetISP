using BuddhaNetISP.DTO;

namespace BuddhaNetISP.Interface
{
    public interface IspecialistRepo
    {
        JsonResponse SaveSpecialist(SpecialistDTO dto);
        JsonResponse UpdateSpecialist(SpecialistDTO dto);
        JsonResponse GetAllSpecialists();
        JsonResponse DeleteSpecialist(int specialistId);
        JsonResponse GetSpecialistById(int specialistId);

    }
}
