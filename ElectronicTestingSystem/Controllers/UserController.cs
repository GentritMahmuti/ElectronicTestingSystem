using ElectronicTestingSystem.Models.DTOs;
using ElectronicTestingSystem.Services;
using ElectronicTestingSystem.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ElectronicTestingSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        //Admins can see users based on their ID.
        [Authorize(Roles = "LifeAdmin")]
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(string Userid)
        {
            var user = await _userService.GetUser(Userid);
            _logger.LogInformation("Get a user");
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        //Admins can see all users.
        [Authorize(Roles = "LifeAdmin")]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsers();
            _logger.LogInformation("Get all the users");
            return Ok(users);
        }

        //Admins can update users.
        [Authorize(Roles = "LifeAdmin")]
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserDto userToUpdate)
        {
            try
            {
                await _userService.UpdateUser(userToUpdate);
                _logger.LogInformation("Updating a user");
                return Ok("User updated successfully!");
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error  updating user data");
                return BadRequest(e.ToString());
            }
        }

        //Admins can delete users. When a user is deleted, the ExamRequest Table's UserID becomes null. 
        [Authorize(Roles = "LifeAdmin")]
        [HttpPost("DeleteUser")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _userService.DeleteUser(id);
                _logger.LogInformation("Deleting a user");
                return Ok("User deleted successfully!");
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error deleting user");
                return BadRequest(e.ToString());
            }
        }
    }
}
