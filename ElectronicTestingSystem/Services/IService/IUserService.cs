using ElectronicTestingSystem.Models.DTOs;

namespace ElectronicTestingSystem.Services.IService
{
    public interface IUserService
    {
        Task CreateUser(UserCreateDto userToCreate);
    }
}
