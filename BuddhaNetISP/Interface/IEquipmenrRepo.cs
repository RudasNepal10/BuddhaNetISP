using BuddhaNetISP.DTO;
using BuddhaNetISP.Models;
namespace BuddhaNetISP.Interface
{
    public interface IEquipmenrRepo
    {
        JsonResponse SaveEquipment(EquipmentDTO dto);
        JsonResponse UpdateEquipment(EquipmentDTO dto);
        JsonResponse GetAllEquipments();
        JsonResponse DeleteEquipment(string serialnumber);
        JsonResponse GetEquipmentById(int equipmentId);
    }
}


