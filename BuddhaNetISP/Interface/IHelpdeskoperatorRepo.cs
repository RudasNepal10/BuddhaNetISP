using BuddhaNetISP.DTO;
using BuddhaNetISP.Models;

namespace BuddhaNetISP.Interface
{
    public interface IHelpdeskoperatorRepo
    {
        JsonResponse SaveHelpDeskOperator(HelpdeskoperatorDTO dto);
        JsonResponse UpdateHelpDeskOperator(HelpdeskoperatorDTO dto);
        JsonResponse GetAllHelpDeskOperators();
        JsonResponse DeleteHelpDeskOperator(int operatorid);
        JsonResponse GetHelpDeskOperatorById(int operatorid);
    }
}
