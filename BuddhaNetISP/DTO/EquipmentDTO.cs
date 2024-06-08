namespace BuddhaNetISP.DTO
{
    public class EquipmentDTO
    {
        public int EquipmentID { get; set; }
        public string SerialNumber { get; set; }
        public string EquipmentType { get; set; }
        public string Make { get; set; }
        public bool IsUnderLicense { get; set; }
        public string Action { get; set; }
    }
}
