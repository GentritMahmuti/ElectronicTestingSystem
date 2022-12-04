using ElectronicTestingSystem.Models.DTOs;
using ElectronicTestingSystem.Services;
using ElectronicTestingSystem.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicTestingSystem.Controllers
{
    [ApiController]
    public class ExamController : Controller
    {
        private readonly IExamService _examService;
        private readonly IEmailSender _emailSender;
        public ExamController(IExamService examService, IEmailSender emailSender)
        {
            _examService = examService;
            _emailSender = emailSender;
        }
        //TakeExam
        [Authorize]
        [HttpGet("TakeExam")]
        public async Task<IActionResult> TakeExam(int id)
        {
            var questions = await _examService.TakeExam(id);
            return Ok(questions);
        }
        [Authorize(Roles="LifeUser")]
        [HttpPost("SubmitExam")]
        public async Task<IActionResult> PostExam(int id,List<int> answers)
        {
            int totalPoints = 0;
            int points=0;
            var questions = await _examService.TakeExam(id);
            for (int i = 0; i < questions.Count; i++)
            {
                totalPoints += questions[i].QuestionPoints;
                if (questions[i].Answer == answers.ElementAt(i))
                {
                    points += questions[i].QuestionPoints;
                }
            }
            double percentage = ((double)points / totalPoints) * 100;
            string result = String.Format("Ju keni plotesuar testin me {0:0.00}% saktesi", percentage);
            // await _emailSender.SendEmailAsync("gentrit.mahmuti@gjirafa.com", "Rezultati", result);
            return Ok(result);
        }
        [Authorize(Roles="LifeAdmin")]
        [HttpPost("PostExam")]
        public async Task<IActionResult> Post(ExamCreateDto examToCreate)
        {
            await _examService.CreateExam(examToCreate);

            return Ok("Exam created successfully!");
        }
        [HttpDelete("DeleteExam")]
        public async Task<IActionResult> Delete(int id)
        {
            await _examService.DeleteExam(id);

            return Ok("Exam deleted successfully!");
        }
    }
}
