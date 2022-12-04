using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronicTestingSystem.Models.Entities
{
    public class UserExam
    {
        [Key]
        public int Id { get; set; }
        public bool IsApproved { get; set; } = false;
        public DateTime? StartTime { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("ExamId")]
        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        
    }
}
