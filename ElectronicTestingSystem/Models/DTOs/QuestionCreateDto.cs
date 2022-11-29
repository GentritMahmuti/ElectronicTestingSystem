using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ElectronicTestingSystem.Models.DTOs
{
    public class QuestionCreateDto
    {
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
        public int Answer { get; set; }
    }
}
