using ElectronicTestingSystem.Models.DTOs;
using ElectronicTestingSystem.Models.Entities;
using System.Threading.Tasks;

namespace ElectronicTestingSystem.Services.IService
{
    public interface IExamService
    {
        Task CreateExam(ExamCreateDto examToCreate);
        Task DeleteExam(int examId);
        Task RequestExam(string userId, int examId);
        Task AproveExam(string userId, int examId);

        Task<List<Question>> TakeApprovedExam(string userId, int examId);
        Task<String> SubmitExamAnswers(int examId, List<int> correctAnswers);
        //Task UpdateExam(ExamDto examToUpdate);
        Task<List<Question>> TakeExam(int examId);
        //Task<Question> GetQuestion(int id);

    }
}
