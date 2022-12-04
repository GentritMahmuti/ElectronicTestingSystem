using System.ComponentModel.DataAnnotations;

namespace ElectronicTestingSystem.Models.Entities
{
    public class User
    {
        public string UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirsName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [EmailAddress]
        [Required]
        [MaxLength()]
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
    }
}
