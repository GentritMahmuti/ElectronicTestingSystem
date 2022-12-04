using AutoMapper;
using ElectronicTestingSystem.Data.UnitOfWork;
using ElectronicTestingSystem.Models.DTOs;
using ElectronicTestingSystem.Models.Entities;
using ElectronicTestingSystem.Services.IService;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
namespace ElectronicTestingSystem.Services
{
    public class QuestionService : IQuestionService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuestionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

            question.QuestionInWords = questionToUpdate.QuestionInWords;
            question.QuestionPoints = questionToUpdate.QuestionPoints;
            question.Option1 = questionToUpdate.Option1;
            question.Option2 = questionToUpdate.Option2;
            question.Option3 = questionToUpdate.Option3;
            question.Option4 = questionToUpdate.Option4;
            question.Answer = questionToUpdate.Answer;
            

            _unitOfWork.Repository<Question>().Update(question);

            _unitOfWork.Complete();
        }

        public async Task DeleteQuestion(int id)
        {
            var question = await GetQuestion(id);

            _unitOfWork.Repository<Question>().Delete(question);

            _unitOfWork.Complete();
        }

        public async Task CreateQuestion(QuestionCreateDto questionToCreate)
        {
            var question = _mapper.Map<Question>(questionToCreate);

            //var question = new Question
            //{
            //    Name = questionToCreate.Name
            //};

            _unitOfWork.Repository<Question>().Create(question);

            _unitOfWork.Complete();
        }

        public async Task CreateMultipleQuestion(List<QuestionCreateDto> multipleQuestionToCreate)
        {
            List<Question> multipleQuestion = new List<Question>();
            foreach (var question in multipleQuestionToCreate)
            {
                multipleQuestion.Add(_mapper.Map<Question>(question));
            }
            _unitOfWork.Repository<Question>().CreateRange(multipleQuestion);

            _unitOfWork.Complete();
        }
        public async Task CreateMultipleQuestionUsingFile(IFormFile file)
        {
            string jsonData = await ReadFormFileAsync(file);
            List<QuestionCreateDto> halo = JsonConvert.DeserializeObject<List<QuestionCreateDto>>(jsonData);
            List<Question> multipleQuestion = new List<Question>();
            foreach (var question in halo)
            {
                multipleQuestion.Add(_mapper.Map<Question>(question));
            }
            _unitOfWork.Repository<Question>().CreateRange(multipleQuestion);

            _unitOfWork.Complete();
        }
        public static async Task<string> ReadFormFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return await Task.FromResult((string)null);
            }

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                return await reader.ReadToEndAsync();
            }
        }

    }
}
