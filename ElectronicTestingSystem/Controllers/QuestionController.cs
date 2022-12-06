using Amazon.S3.Model;
using Amazon.S3;
using ElectronicTestingSystem.Models.DTOs;
using ElectronicTestingSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using ElectronicTestingSystem.Models.Entities;
using System.Runtime.Intrinsics.X86;
using ElectronicTestingSystem.Migrations;

namespace ElectronicTestingSystem.Controllers
{
    [ApiController]
    public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly ILogger<QuestionController> _logger;

        public QuestionController(IQuestionService questionService, ILogger<QuestionController> logger)
        {
            _questionService = questionService;
            _logger = logger;
        }

        //Admins can see questions based on their ID.
        [Authorize(Roles = "LifeAdmin")]
        [HttpGet("GetQuestion")]
        public async Task<IActionResult> Get(int id)
        {
            var question = await _questionService.GetQuestion(id);
            _logger.LogInformation("Getting a question");
            if (question == null)
            {
                return NotFound();
            }

            return Ok(question);
        }

        //Admin can see all the questions.
        [Authorize(Roles = "LifeAdmin")]
        [HttpGet("GetAllQuestions")]
        public async Task<IActionResult> GetQuestions()
        {
            var questions = await _questionService.GetAllQuestions();
            _logger.LogInformation("Getting all questions");
            return Ok(questions);
        }

        //Admin can create questions.
        [Authorize(Roles = "LifeAdmin")]
        [HttpPost("CreateQuestion")]
        public async Task<IActionResult> CreateQuestion(QuestionCreateDto QuestionToCreate)
        {
            await _questionService.CreateQuestion(QuestionToCreate);
            _logger.LogInformation("Creating a question");
            return Ok("Question created successfully!");
        }
        //Admins can create multiple questions at the same time.
        [Authorize(Roles = "LifeAdmin")]
        [HttpPost("CreateMultipleQuestions")]
        public async Task<IActionResult> PostMultipleQuestion(List<QuestionCreateDto> MultipleQuestionsToCreate)
        {
            await _questionService.CreateMultipleQuestions(MultipleQuestionsToCreate);
            _logger.LogInformation("Creating multiple questions");
            return Ok("Questions created successfully!");
        }

        //Admins can create multiple questions at the same time using json file
        [Authorize(Roles = "LifeAdmin")]
        [HttpPost("CreateMultipleQuestionsFromFile")]
        public async Task<IActionResult> PostMultipleQuestionFromFile(IFormFile file)
        {
            await _questionService.CreateMultipleQuestionsUsingFile(file);
            _logger.LogInformation("Creating multiple questions using file");
            return Ok("Questions created successfully!");
        }

        //Admin can update questions.
        [Authorize(Roles = "LifeAdmin")]
        [HttpPut("UpdateQuestion")]
        public async Task<IActionResult> Update(QuestionDto QuestionToUpdate)
        {
            try
            {
                await _questionService.UpdateQuestion(QuestionToUpdate);
                _logger.LogInformation("Updating question");
                return Ok("Question updated successfully!");
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error  updating question");
                return BadRequest(e.ToString());
            }
        }

        //Admin can delete questions.
        [Authorize(Roles = "LifeAdmin")]
        [HttpDelete("DeleteQuestion")]
        public async Task<IActionResult> Delete(int id)
        {
            await _questionService.DeleteQuestion(id);
            _logger.LogInformation("Deleting question");
            return Ok("Question deleted successfully!");
        }

        //Admin can upload Image using url 
        [HttpPost("UploadImageUsingUrl")]
        [Authorize(Roles = "LifeAdmin")]
        public async Task<IActionResult> UploadImageUsingUrl(int questionId, string url)
        {
            try
            {
                var imageUrl = await _questionService.UploadImageFromUrl(url, questionId);
                return Ok($"The image was downloaded and uploaded at : {imageUrl}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in downloading and uploading image.");
                return BadRequest(e.ToString());
            }
        }

        //Admin can upload a local Image 
        [HttpPost("UploadImage")]
        [Authorize(Roles = "LifeAdmin")]
        public async Task<IActionResult> UploadImage(IFormFile file, int questionId)
        {
            try
            {
                var url = await _questionService.UploadImage(file, questionId);
                return Ok($"\"The image was uploaded at the url: {url}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in uploading the image.");
                return BadRequest(ex.ToString());
            }

        }
    }
}
