using System.ComponentModel.DataAnnotations;

namespace ElectronicTestingSystem.Models.DTOs
{
    public class ExamCreateDto
    {
        [Required]
        public int NrOfQuestions { get; set; }

    }
}
