using ElectronicTestingSystem.Models.DTOs;
using ElectronicTestingSystem.Models.Entities;

namespace ElectronicTestingSystem.Services.IService
{
    public interface IQuestionService
    {
        Task CreateQuestion(QuestionCreateDto questionToCreate);
        Task CreateMultipleQuestions(List<QuestionCreateDto> multipleQuestionToCreate);
        Task CreateMultipleQuestionsUsingFile(IFormFile file);
        Task DeleteQuestion(int id);
        Task<List<Question>> GetAllQuestions();
        Task<Question> GetQuestion(int id);
        Task UpdateQuestion(QuestionDto questionToUpdate);
        Task<string> UploadImage(IFormFile? file, int questionId);
        Task<string> UploadImageFromUrl(string url, int questionId);
    }
}
