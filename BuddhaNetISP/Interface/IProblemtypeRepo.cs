using BuddhaNetISP.DTO;

namespace BuddhaNetISP.Interface
{
    public interface IProblemtypeRepo
    {
      JsonResponse SaveProblemType(ProblemtypeDTO dto);
      JsonResponse UpdateProblemType(ProblemtypeDTO dto);
      JsonResponse GetAllProblemTypes();
      JsonResponse DeleteProblemType(int problemtypeid);
      JsonResponse GetProblemTypeById(int problemtypeid);
    }
}
