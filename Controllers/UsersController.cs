using Bank;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.IdentityModel.Tokens;
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
        private const string PostCreateUser = "/users";
        private const string PutUpdateUser = "/users/{id}";

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

        [HttpPost(PostCreateUser, Name = nameof(PostCreateUserAsync))]
        public async Task<IActionResult> PostCreateUserAsync([FromBody] BankUser user)
        {
            Result<BankUser> createResult = await _userRepository.CreateUserAsync(user);
            if (createResult.Succeeded == false)
            {
                return BadRequest();
            }
            return Ok(createResult.Value);
        }

        [HttpPut(PutUpdateUser, Name = nameof(PutUpdateUserAsync))]
        public async Task<IActionResult> PutUpdateUserAsync(string id, [FromBody] BankUser user)
        {
            if (id != user.Id)
            {
                return BadRequest("Parameter 'id' does not match id from request body");
            }

            Result<BankUser> updateResult = await _userRepository.UpdateUserAsync(user);
            if (updateResult.Succeeded == false)
            {
                return BadRequest();
            }
            return Ok(updateResult.Value);
        }
    }
}
