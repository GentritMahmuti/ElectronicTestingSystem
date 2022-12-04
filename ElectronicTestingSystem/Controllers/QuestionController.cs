using Amazon.S3.Model;
using Amazon.S3;
using ElectronicTestingSystem.Models.DTOs;
using ElectronicTestingSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ElectronicTestingSystem.Controllers
{
    [ApiController]
    public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<QuestionController> _logger;

        public QuestionController(IQuestionService questionService, IConfiguration configuration, ILogger<QuestionController> logger)
        {
            _questionService = questionService;
            _configuration = configuration;
            _logger = logger;
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

        //[HttpGet("TestSerilog")]
        //public async Task<IActionResult> TestSerilog(int id)
        //{
           
        //        int num = 4;
        //        int num2 = 0;

        //        int num3 = num / num2;
            
        //    return Ok("Tested");
        //}

        [HttpGet("GetQuestions")]
        public async Task<IActionResult> GetQuestions()
        {
            var questions = await _questionService.GetAllQuestions();

            return Ok(questions);
        }

        [HttpPost("CreateQuestion")]
        public async Task<IActionResult> Post(QuestionCreateDto QuestionToCreate)
        {
            await _questionService.CreateQuestion(QuestionToCreate);

            return Ok("Question created successfully!");
        }

        [HttpPost("CreateMultipleQuestions")]
        public async Task<IActionResult> PostMultipleQuestion(List<QuestionCreateDto> MultipleQuestionsToCreate)
        {
            await _questionService.CreateMultipleQuestion(MultipleQuestionsToCreate);

            return Ok("Questions created successfully!");
        }
        [HttpPost("CreateMultipleQuestionsFromFile")]
        public async Task<IActionResult> PostMultipleQuestionFromFile(IFormFile file)
        {
            await _questionService.CreateMultipleQuestionUsingFile(file);

            return Ok("Questions created successfully!");
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
        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var uploadPicture = await UploadToBlob(file);

            var imageUrl = $"{_configuration.GetValue<string>("BlobConfig:CDNLife")}{file.FileName}";

            return Ok(imageUrl);
        }

        [NonAction]
        public async Task<PutObjectResponse> UploadToBlob(IFormFile file)
        {

            string serviceURL = _configuration.GetValue<string>("BlobConfig:serviceURL");
            string AWS_accessKey = _configuration.GetValue<string>("BlobConfig:accessKey");
            string AWS_secretKey = _configuration.GetValue<string>("BlobConfig:secretKey");
            var bucketName = _configuration.GetValue<string>("BlobConfig:bucketName");
            var keyName = _configuration.GetValue<string>("BlobConfig:defaultFolder");

            var config = new AmazonS3Config() { ServiceURL = serviceURL };
            var s3Client = new AmazonS3Client(AWS_accessKey, AWS_secretKey, config);
            keyName = String.Concat(keyName, file.FileName);

            var fs = file.OpenReadStream();
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = keyName,
                InputStream = fs,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.PublicRead
            };

            return await s3Client.PutObjectAsync(request);
        }

    }
}
