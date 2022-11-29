using System.ComponentModel.DataAnnotations;

namespace ElectronicTestingSystem.Models.DTOs
{
    public class UserCreateDto
    {
       
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
