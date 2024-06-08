namespace BuddhaNetISP.Models
{
    public class Problemtype:BaseClass
    {
     public int Problemtypeid {  get; set; } 
     public string Typename {  get; set; }  
     public int ParentproblemTypeId {  get; set; }  
    }
}
