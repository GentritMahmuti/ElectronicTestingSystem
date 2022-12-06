using Amazon.S3.Model;
using Amazon.S3;
using AutoMapper;
using ElectronicTestingSystem.Data.UnitOfWork;
using ElectronicTestingSystem.Models.DTOs;
using ElectronicTestingSystem.Models.Entities;
using ElectronicTestingSystem.Services.IService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
namespace ElectronicTestingSystem.Services
{
    public class QuestionService : IQuestionService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public QuestionService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<List<Question>> GetAllQuestions()
        {
            var questions = _unitOfWork.Repository<Question>().GetAll();

            return questions.ToList();
        }

        public async Task<Question> GetQuestion(int id)
        {
            Expression<Func<Question, bool>> expression = x => x.QuestionId == id;
            var question = await _unitOfWork.Repository<Question>().GetById(expression).FirstOrDefaultAsync();

            return question;
        }

        public async Task UpdateQuestion(QuestionDto questionToUpdate)
        {
            Question? question = await GetQuestion(questionToUpdate.QuestionId);
            if (question == null)
            {
                throw new Exception("A question with this ID doesn't exist.");
            }

            question.QuestionInWords = questionToUpdate.QuestionInWords;
            question.QuestionPoints = questionToUpdate.QuestionPoints;
            question.Option1 = questionToUpdate.Option1;
            question.Option2 = questionToUpdate.Option2;
            question.Option3 = questionToUpdate.Option3;
            question.Option4 = questionToUpdate.Option4;
            question.CorrectAnswer = questionToUpdate.CorrectAnswer;


            _unitOfWork.Repository<Question>().Update(question);

            _unitOfWork.Complete();
        }

        public async Task DeleteQuestion(int id)
        {
            var question = await GetQuestion(id);
            if (question == null)
            {
                throw new Exception("A question with this ID doesn't exist.");
            }
            _unitOfWork.Repository<Question>().Delete(question);

            _unitOfWork.Complete();
        }

        public async Task CreateQuestion(QuestionCreateDto questionToCreate)
        {
            var question = _mapper.Map<Question>(questionToCreate);

            _unitOfWork.Repository<Question>().Create(question);

            _unitOfWork.Complete();
        }

        public async Task CreateMultipleQuestions(List<QuestionCreateDto> multipleQuestionsToCreate)
        {
            var questions = _mapper.Map<List<QuestionCreateDto>, List<Question>>(multipleQuestionsToCreate);
            _unitOfWork.Repository<Question>().CreateRange(questions);

            _unitOfWork.Complete();
        }
        public async Task CreateMultipleQuestionsUsingFile(IFormFile file)
        {
            string jsonData = await ReadFromFileAsync(file);
            List<QuestionCreateDto> questionCreateDto = JsonConvert.DeserializeObject<List<QuestionCreateDto>>(jsonData);
            await CreateMultipleQuestions(questionCreateDto);

        }
        public static async Task<string> ReadFromFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new Exception("Given File is null or empty!");
            }

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                return await reader.ReadToEndAsync();
            }
        }
        public async Task<string> UploadImage(IFormFile? file, int questionId)
        {
            var uploadPicture = await UploadToBlob(file, file.FileName, Path.GetExtension(file.FileName));
            var imageUrl = $"{_configuration.GetValue<string>("BlobConfig:CDNLife")}{file.FileName}";

            var question = await GetQuestion(questionId);
            if (question is null)
            {
                throw new Exception("A question with this ID doesn't exist.");
            }
            question.ImageUrl = imageUrl;

            _unitOfWork.Repository<Question>().Update(question);
            _unitOfWork.Complete();
            return imageUrl;
        }


        public async Task<string> UploadImageFromUrl(string url, int questionId)
        {
            if (url == null)
            {
                throw new Exception("The url is null!");
            }
            var httpClient = new HttpClient();
            HttpResponseMessage res = await httpClient.GetAsync(url.Replace("%2F", "/"));
            byte[] content = await res.Content.ReadAsByteArrayAsync();
            var extension = Path.GetExtension(url);
            var imageUri = Guid.NewGuid() + extension;
            var stream = new MemoryStream(content);
            IFormFile file = new FormFile(stream, 0, content.Length, null, imageUri);
            await UploadToBlob(file, imageUri, extension);
            var urlImage = $"{_configuration.GetValue<string>("BlobConfig:CDNLife")}{imageUri}";

            var question = await GetQuestion(questionId);
            if (question is null)
            {
                throw new NullReferenceException("TA question with this ID doesn't exist.");
            }

            question.ImageUrl = urlImage;
            _unitOfWork.Repository<Question>().Update(question);
            _unitOfWork.Complete();
            return urlImage;
        }


        public async Task<PutObjectResponse> UploadToBlob(IFormFile? file, string name, string extension)
        {
            string serviceURL = _configuration.GetValue<string>("BlobConfig:serviceURL");
            string AWS_accessKey = _configuration.GetValue<string>("BlobConfig:accessKey");
            string AWS_secretKey = _configuration.GetValue<string>("BlobConfig:secretKey");
            var bucketName = _configuration.GetValue<string>("BlobConfig:bucketName");
            var keyName = _configuration.GetValue<string>("BlobConfig:defaultFolder");

            var config = new AmazonS3Config() { ServiceURL = serviceURL };
            var s3Client = new AmazonS3Client(AWS_accessKey, AWS_secretKey, config);
            keyName = String.Concat(keyName, name);

            var fs = file.OpenReadStream();
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = keyName,
                InputStream = fs,
                ContentType = $"image/{extension}",
                CannedACL = S3CannedACL.PublicRead
            };
            return await s3Client.PutObjectAsync(request);
        }
    }
}
