using System.ComponentModel.DataAnnotations;

namespace BuddhaNetISP.DTO
{
    public class HelpdeskqueriesDTO
    {

        public int QueryId { get; set; }

        public int CallerId { get; set; }

        public int OperatorId { get; set; }

        
        public DateTime CallTime { get; set; }

        public int EquipmentId { get; set; }

        public int SoftwareId { get; set; }

        
        public int ProblemTypeId { get; set; }

       
        public string Description { get; set; }

       
        public bool IsResolved { get; set; }

        public DateTime? ResolutionTime { get; set; }

        
        public string ResolutionDetails { get; set; }

      
        public int SpecialistId { get; set; }

        
        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
