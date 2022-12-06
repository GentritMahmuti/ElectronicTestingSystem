using ElectronicTestingSystem.Models.DTOs;
using ElectronicTestingSystem.Models.Entities;

namespace ElectronicTestingSystem.Services.IService
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUser(string id);
        Task UpdateUser(UserDto userToUpdate);
        Task DeleteUser(string id);
    }
}
