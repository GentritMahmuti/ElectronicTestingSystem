using ElectronicTestingSystem.Models.DTOs;
using ElectronicTestingSystem.Services;
using ElectronicTestingSystem.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ElectronicTestingSystem.Controllers
{
    [ApiController]
    public class ExamController : Controller
    {
        private readonly IExamService _examService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExamController> _logger;
        public ExamController(IExamService examService, IEmailSender emailSender, ILogger<ExamController> logger)
        {
            _examService = examService;
            _emailSender = emailSender;
            _logger = logger;
        }

        //The user requests to take an exam. 
        [Authorize(Roles = "LifeUser")]
        [HttpGet("RequestExam")]
        public async Task<IActionResult> RequestExam(int examId)
        {
            var userData = (ClaimsIdentity)User.Identity;
            var userId = userData.FindFirst(ClaimTypes.NameIdentifier).Value;
            _logger.LogInformation("Requesting an exam");
            await _examService.RequestExam(userId,examId);
            return Ok("Exam requested");

        }

        //An exam can be approved by admin so that a user can take it. 
        [Authorize(Roles = "LifeAdmin")]
        [HttpGet("ApproveExam")]
        public async Task<IActionResult> ApproveExam(string userId, int examId)
        {
            _examService.AproveExam(userId, examId);
            _logger.LogInformation("Approving an Exam");
            return Ok("Exam approved");

        }

        //An exam can be taken after the user's request has been approved by the admin.
        [Authorize(Roles = "LifeUser")]
        [HttpGet("TakeExam")]
        public async Task<IActionResult> TakeExam(int id)
        {
            var userData = (ClaimsIdentity)User.Identity;
            var userId = userData.FindFirst(ClaimTypes.NameIdentifier).Value;
            var questions = await _examService.TakeApprovedExam(userId, id);
            _logger.LogInformation("Taking an exam");
            return Ok(questions);
        }
        //The user can submit his answers after he takes an exam.
        [Authorize(Roles = "LifeUser")]
        [HttpPost("SubmitExam")]
        public async Task<IActionResult> SubmitExamAnswers(int examId,List<int> correctAnswers)
        {
            string result = await _examService.SubmitExamAnswers(examId, correctAnswers);
            //await _emailSender.SendEmailAsync("gentrit.mahmuti@gjirafa.com", "Rezultati", result);
            _logger.LogInformation("Submitting an exam");
            return Ok(result);
        }

        //Admin can create an exam.
        [Authorize(Roles = "LifeAdmin")]
        [HttpPost("CreateExam")]
        public async Task<IActionResult> CreateExam(ExamCreateDto examToCreate)
        {
            await _examService.CreateExam(examToCreate);
            _logger.LogInformation("Creating an exam");
            return Ok("Exam created successfully!");
        }

        //Admin can delete an exam.
        [Authorize(Roles = "LifeAdmin")]
        [HttpDelete("DeleteExam")]
        public async Task<IActionResult> Delete(int id)
        {
            await _examService.DeleteExam(id);
            _logger.LogInformation("Deleting an exam");
            return Ok("Exam deleted successfully!");
        }
    }
}
