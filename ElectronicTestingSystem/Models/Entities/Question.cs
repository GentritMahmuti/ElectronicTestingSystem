using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ElectronicTestingSystem.Models.Entities
{
    public class Question
    {
        public Question()
        {
            Exams = new HashSet<Exam>();
        }
        [Key]
        public int QuestionId { get; set; }
        public int QuestionPoints { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string QuestionInWords { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string ImageUrl { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Option1 { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Option2 { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Option3 { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Option4 { get; set; }
        [JsonIgnore]
        public int Answer { get; set; }

        public virtual ICollection<Exam> Exams { get; set; }

    }
}
