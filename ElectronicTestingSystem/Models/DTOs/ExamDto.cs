using System.ComponentModel.DataAnnotations;

namespace ElectronicTestingSystem.Models.DTOs
{
    public class ExamDto
    {
        [Key]
        public int ExamId { get; set; }
        [Required]
        public int NrOfQuestions { get; set; }
    }
}
