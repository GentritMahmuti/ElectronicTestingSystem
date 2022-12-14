using System.ComponentModel.DataAnnotations;

namespace ElectronicTestingSystem.Models.Entities
{
    public class Exam
    {
        public Exam()
        {
            Questions = new HashSet<Question>();
        }
        [Key]
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public string ExamAuthor { get; set; }
        [Required]
        public int NrOfQuestions { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
