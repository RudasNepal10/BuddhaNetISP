using BuddhaNetISP.DTO;

namespace BuddhaNetISP.Interface
{
    public interface ISoftwareRepo
    {
        JsonResponse SaveSoftware(SoftwareDTO dto);
        JsonResponse UpdateSoftware(SoftwareDTO dto);
        JsonResponse GetAllSoftware();
        JsonResponse DeleteSoftware(int softwareid);
        JsonResponse GetSoftwareById(int softwareid);
    }
}
