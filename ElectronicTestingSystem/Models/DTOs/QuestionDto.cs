using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ElectronicTestingSystem.Models.DTOs
{
    public class QuestionDto
    {
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
        [Range(1, 4)]
        public int CorrectAnswer { get; set; }
    }
}
