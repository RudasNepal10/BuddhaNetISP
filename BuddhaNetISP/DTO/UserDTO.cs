using System.ComponentModel.DataAnnotations;

namespace BuddhaNetISP.DTO
{
    public class UserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Errors { get; set; }
    }
}
