using ElectronicTestingSystem.Models.DTOs;
using ElectronicTestingSystem.Services;
using ElectronicTestingSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicTestingSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Post(UserCreateDto userToCreate)
        {
            await _userService.CreateUser(userToCreate);

            return Ok("User created successfully!");
        }
    }
}
