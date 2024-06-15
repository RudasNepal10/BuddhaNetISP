using BuddhaNetISP.DTO;

namespace BuddhaNetISP.Interface
{
    public interface IHelpdeskqueriesRepo
    {
      JsonResponse SaveHelpdeskQuery(HelpdeskqueriesDTO dto);
      JsonResponse GetAllHelpdeskQueries();
      JsonResponse DeleteHelpdeskQuery(int queryId);
    }
}
