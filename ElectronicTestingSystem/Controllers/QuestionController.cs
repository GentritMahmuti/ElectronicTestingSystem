using ElectronicTestingSystem.Models.DTOs;
using ElectronicTestingSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicTestingSystem.Controllers
{
    [ApiController]
    public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }


        [HttpGet("GetQuestion")]
        public async Task<IActionResult> Get(int id)
        {
            var question = await _questionService.GetQuestion(id);

            if (question == null)
            {
                return NotFound();
            }

            return Ok(question);
        }

        [HttpGet("GetQuestions")]
        public async Task<IActionResult> GetQuestions()
        {
            var questions = await _questionService.GetAllQuestions();

            return Ok(questions);
        }

        [HttpPost("PostQuestion")]
        public async Task<IActionResult> Post(QuestionCreateDto QuestionToCreate)
        {
            await _questionService.CreateQuestion(QuestionToCreate);

            return Ok("Question created successfully!");
        }

        [HttpPut("UpdateQuestion")]
        public async Task<IActionResult> Update(QuestionDto QuestionToUpdate)
        {
            await _questionService.UpdateQuestion(QuestionToUpdate);

            return Ok("Question updated successfully!");
        }

        [HttpDelete("DeleteQuestion")]
        public async Task<IActionResult> Delete(int id)
        {
            await _questionService.DeleteQuestion(id);

            return Ok("Question deleted successfully!");
        }

    }
}
