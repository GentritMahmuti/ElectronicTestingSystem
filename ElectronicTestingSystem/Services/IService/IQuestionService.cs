using ElectronicTestingSystem.Models.DTOs;
using ElectronicTestingSystem.Models.Entities;

namespace ElectronicTestingSystem.Services.IService
{
    public interface IQuestionService
    {
        Task CreateQuestion(QuestionCreateDto questionToCreate);
        Task CreateMultipleQuestion(List<QuestionCreateDto> multipleQuestionToCreate);
        Task CreateMultipleQuestionUsingFile(IFormFile file);
        Task DeleteQuestion(int id);
        Task<List<Question>> GetAllQuestions();
        Task<Question> GetQuestion(int id);
        Task UpdateQuestion(QuestionDto questionToUpdate);
    }
}
