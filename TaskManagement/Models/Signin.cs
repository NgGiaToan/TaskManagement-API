using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models
{
    public class Signin
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

}
