using BuddhaNetISP.DTO;

namespace BuddhaNetISP.Interface
{
    public interface IuserRepo
    {
        JsonResponse GetUser(UserDTO userDTO);
    }
}
