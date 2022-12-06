using AutoMapper;
using ElectronicTestingSystem.Data.UnitOfWork;
using ElectronicTestingSystem.Models.DTOs;
using ElectronicTestingSystem.Models.Entities;
using ElectronicTestingSystem.Services.IService;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ElectronicTestingSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = _unitOfWork.Repository<User>().GetAll();

            return users.ToList();
        }

        public async Task<User> GetUser(string id)
        {
            Expression<Func<User, bool>> expression = x => x.UserId.Equals(id);
            var user = await _unitOfWork.Repository<User>().GetById(expression).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new Exception("A user with this ID doesn't exist.");
            }

            return user;
        }

        public async Task UpdateUser(UserDto userToUpdate)
        {
            User? user = await GetUser(userToUpdate.UserId);

            if (user == null)
            {
                throw new Exception("A user with this ID doesn't exist.");
            }

            user.FirsName = userToUpdate.FirsName;
            user.LastName = userToUpdate.LastName;
            user.Email = userToUpdate.Email;
            user.DateOfBirth = userToUpdate.DateOfBirth;
            user.Gender = userToUpdate.Gender;
            user.PhoneNumber = userToUpdate.PhoneNumber;


            _unitOfWork.Repository<User>().Update(user);

            _unitOfWork.Complete();
        }
        public async Task DeleteUser(string id)
        {

            var user = await GetUser(id);
            if (user == null)
            {
                throw new Exception("A user with this ID doesn't exist.");
            }
            var examRequests = _unitOfWork.Repository<ExamRequest>().GetById(x => x.UserId.Equals(id));
            foreach (var examRequest in examRequests)
            {
                examRequest.UserId = null;
                _unitOfWork.Repository<ExamRequest>().Update(examRequest);
            }
            _unitOfWork.Repository<User>().Delete(user);
            _unitOfWork.Complete();
        }
    }
}
