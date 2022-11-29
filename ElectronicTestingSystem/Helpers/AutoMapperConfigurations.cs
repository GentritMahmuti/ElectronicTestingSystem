using AutoMapper;
using ElectronicTestingSystem.Models.DTOs;
using ElectronicTestingSystem.Models.Entities;

namespace ElectronicTestingSystem.Helper
{
    public class AutoMapperConfigurations : Profile
    {
        public AutoMapperConfigurations()
        {
            CreateMap<Question, QuestionDto>().ReverseMap();
            CreateMap<Question, QuestionCreateDto>().ReverseMap();

            CreateMap<Exam, ExamDto>().ReverseMap();
            CreateMap<Exam, ExamCreateDto>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserCreateDto>().ReverseMap();


        }
    }
}
