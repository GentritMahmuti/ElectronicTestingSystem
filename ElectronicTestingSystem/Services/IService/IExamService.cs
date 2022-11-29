using ElectronicTestingSystem.Models.DTOs;
using ElectronicTestingSystem.Models.Entities;

namespace ElectronicTestingSystem.Services.IService
{
    public interface IExamService
    {
        Task CreateExam(ExamCreateDto examToCreate);
        Task DeleteExam(int id);
        //Task UpdateExam(ExamDto examToUpdate);
        Task<List<Question>> TakeExam(int id);
        //Task<Question> GetQuestion(int id);

    }
}
