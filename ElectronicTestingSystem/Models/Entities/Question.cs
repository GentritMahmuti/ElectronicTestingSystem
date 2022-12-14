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
        [Range(1,20)]
        public int QuestionPoints { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string QuestionInWords { get; set; }
        public string ImageUrl { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Option1 { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Option2 { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Option3 { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Option4 { get; set; }
        [JsonIgnore]
        [Range(1, 4)]
        public int CorrectAnswer { get; set; }
        [JsonIgnore]
        public virtual ICollection<Exam> Exams { get; set; }

    }
}
