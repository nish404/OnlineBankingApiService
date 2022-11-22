using Bank;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Models;

namespace OnlineBankingApiService.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        // Routes
        private const string GetAllUsers = "/users";
        private const string GetUserByUserName = "/users/{userName}";

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet(GetAllUsers, Name = nameof(GetAllUsersAsync))]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            Result<List<BankUser>> getAllResult = await _userRepository.GetAllUsersAsync();
            if (getAllResult.Succeeded == false)
            {
                return BadRequest();
            }
            return Ok(getAllResult.Value);
        }

        [HttpGet(GetUserByUserName, Name = nameof(GetUserByUserNameAsync))]
        public async Task<IActionResult> GetUserByUserNameAsync(string userName)
        {
            Result<BankUser> getResult = await _userRepository.GetByUserNameAsync(userName);
            if (getResult.Succeeded == false)
            {
                return BadRequest();
            }
            return Ok(getResult.Value);
        }
    }
}
