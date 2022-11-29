using AutoMapper;
using ElectronicTestingSystem.Data.UnitOfWork;
using ElectronicTestingSystem.Models.DTOs;
using ElectronicTestingSystem.Models.Entities;
using ElectronicTestingSystem.Services.IService;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ElectronicTestingSystem.Services
{
    public class ExamService : IExamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExamService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task CreateExam(ExamCreateDto examToCreate)
        {
            var exam = _mapper.Map<Exam>(examToCreate);

            Exam myExam = new Exam
            {
                NrOfQuestions = exam.NrOfQuestions,
                Questions = _unitOfWork.Repository<Question>().GetAll().OrderBy(r => Guid.NewGuid()).Take(exam.NrOfQuestions).ToList()
            };

            _unitOfWork.Repository<Exam>().Create(myExam);

            _unitOfWork.Complete();
        }
        public async Task DeleteExam(int id)
        {
            Expression<Func<Exam, bool>> expression = x => x.ExamId == id;
            var exam = await _unitOfWork.Repository<Exam>().GetById(expression).FirstOrDefaultAsync();

            _unitOfWork.Repository<Exam>().Delete(exam);

            _unitOfWork.Complete();
        }

        public async Task<List<Question>> TakeExam(int id)
        {
            
            var questions = _unitOfWork.Repository<Question>().GetAll();
            var query = from question in questions
                        where question.Exams.Any(c => c.ExamId == id)
                        select question;

            return query.ToList();
        }

    }
}
