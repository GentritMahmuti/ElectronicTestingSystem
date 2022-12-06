using AutoMapper;
using ElectronicTestingSystem.Data.UnitOfWork;
using ElectronicTestingSystem.Models.DTOs;
using ElectronicTestingSystem.Models.Entities;
using ElectronicTestingSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using System.Security.Claims;

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

        public async Task RequestExam(string userId, int examId)
        {

            var examUser = _unitOfWork.Repository<ExamRequest>().GetAll().Where(x => (x.UserId.Equals(userId) && x.ExamId == examId)).FirstOrDefault();
            if (examUser != null)
            {
                throw new Exception("This exam has been requested by this user before.");
            }
            ExamRequest userExam = new ExamRequest
            {
                IsApproved = false,
                UserId = userId,
                ExamId = examId,
            };
            _unitOfWork.Repository<ExamRequest>().Create(userExam);
            _unitOfWork.Complete();

        }
        public async Task CreateExam(ExamCreateDto examToCreate)
        {
            var exam = _mapper.Map<Exam>(examToCreate);

            Exam myExam = new Exam
            {
                NrOfQuestions = exam.NrOfQuestions,
                ExamName = exam.ExamName,
                ExamAuthor = exam.ExamAuthor,
                Questions = _unitOfWork.Repository<Question>().GetAll().OrderBy(r => Guid.NewGuid()).Take(exam.NrOfQuestions).ToList()
            };

            _unitOfWork.Repository<Exam>().Create(myExam);

            _unitOfWork.Complete();
        }
        public async Task DeleteExam(int id)
        {

            Expression<Func<Exam, bool>> expression = x => x.ExamId == id;
            var exam = await _unitOfWork.Repository<Exam>().GetById(expression).FirstOrDefaultAsync();
            if (exam == null)
            {
                throw new Exception("Exam not found!");
            }
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

        public async Task AproveExam(string userId, int examId)
        {

            var examRequest = await GetExamRequest(userId, examId);
            if (examRequest == null)
            {
                throw new Exception("This exam request doesn't exists.");
            }
            examRequest.IsApproved = true;
            _unitOfWork.Repository<ExamRequest>().Update(examRequest);
            _unitOfWork.Complete();

        }
        public async Task<List<Question>> TakeApprovedExam(string userId, int examId)
        {
            var examRequest = await GetExamRequest(userId, examId);
            if (examRequest == null)
            {
                throw new Exception("This exam request doesn't exists.");
            }
            if (examRequest.StartTime != null)
            {
                throw new Exception("This user took this exam before");
            }
            if (!examRequest.IsApproved)
            {
                throw new AccessViolationException("User can't take this exam because his request is not approved");
            }
            examRequest.StartTime = DateTime.Now;
            _unitOfWork.Repository<ExamRequest>().Update(examRequest);
            _unitOfWork.Complete();
            var questions = await TakeExam(examId);
            return questions;


        }
        public async Task<ExamRequest> GetExamRequest(string userId, int id)
        {
            var examRequest = _unitOfWork.Repository<ExamRequest>().GetAll().Where(x => (x.UserId.Equals(userId) && x.ExamId == id)).FirstOrDefault();
            if(examRequest == null)
            {
                throw new Exception("Exam request not found!");
            }
            return examRequest;
        }
        public async Task<String> SubmitExamAnswers(int examId, List<int> correctAnswers)
        {
            int grade ;
            int totalPoints = 0;
            int points = 0;
            var questions = await TakeExam(examId);
            if(questions.Count != correctAnswers.Count)
            {
                throw new Exception("The number of questions does not equal the number of the answers.");
            }
            for (int i = 0; i < questions.Count; i++)
            {
                totalPoints += questions[i].QuestionPoints;
                if (questions[i].CorrectAnswer == correctAnswers.ElementAt(i))
                {
                    points += questions[i].QuestionPoints;
                }
            }
            double percentage = ((double)points / totalPoints) * 100;
            if(percentage<50)
            {
                grade = 5;
            }
            else if(percentage>=50 && percentage <60)
            {
                grade = 6;
            }
            else if (percentage >= 60 && percentage < 70)
            {
                grade = 7;
            }
            else if (percentage >= 70 && percentage < 80)
            {
                grade = 8;
            }
            else if (percentage >= 80 && percentage < 90)
            {
                grade = 9;
            }
            else
            {
                grade = 10;
            }
            string result = String.Format("Your answers are {0:0.00}% correct , and your grade is {1}. You will receive an email with your grade.", percentage,grade);
            return result;
        }

    }
}
