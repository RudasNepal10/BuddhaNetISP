using System.ComponentModel.DataAnnotations;

namespace BuddhaNetISP.Models
{
    public class Helpdeskqueries:BaseClass
    {
     public int queryid {  get; set; }  
     public int callerid {  get; set; } 
     public int operatorid {  get; set; }   
     public TimestampAttribute calltime { get; set; }   
     public int Equipmentid {  get; set; }  
     public int Softwareid { get; set;}
     public int Problemtypeid {  get; set; }    
     public string description {  get; set; }   
     public bool isresolved {  get; set; }
     public TimestampAttribute resolutiontime { get; set; } 
     public string resolutiondetails {  get; set; } 
     public int specialistid {  get; set; } 
     public TimestampAttribute createdate { get; set; } 
     public TimestampAttribute updatedate { get; set;}

    }
}
