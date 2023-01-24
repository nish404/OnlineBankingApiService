using Bank;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using OnlineBankingProject.Common.Models;
using System.Security.Principal;

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
        private const string GetUser = "/users/{userName}/{id}";
        private const string PostCreateUser = "/users";
        private const string PutUpdateUser = "/users/{id}";
        private const string DeleteBankUser = "/users/{userName}/{id}";

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet(GetUser, Name = nameof(GetUserAsync))]
        public async Task<IActionResult> GetUserAsync(string userName, string id)
        {
            Result<BankUser> getUser = await _userRepository.GetByIdAsync(id);
            if (getUser.Succeeded == false)
            {
                switch (getUser.ResultType)
                {
                    case ResultType.NotFound:
                        return NotFound(getUser.Message);
                    case ResultType.InvalidData:
                        return BadRequest(getUser.Message);
                    case ResultType.DataStoreError:
                        return Conflict(getUser.Message);
                    case ResultType.Duplicate:
                        return Conflict(getUser.Message);
                    default:
                        return StatusCode(500);
                }
            }
            return Ok(getUser.Value);
        }

        [HttpGet(GetAllUsers, Name = nameof(GetAllUsersAsync))]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            Result<List<BankUser>> getAllResult = await _userRepository.GetAllUsersAsync();
            if (getAllResult.Succeeded == false)
            {
                switch (getAllResult.ResultType)
                {
                    case ResultType.NotFound:
                        return NotFound();
                    case ResultType.InvalidData:
                        return BadRequest(getAllResult.Message);
                    case ResultType.DataStoreError:
                        return Conflict(getAllResult.Message);
                    case ResultType.Duplicate:
                        return Conflict(getAllResult.Message);
                    default:
                        return StatusCode(500);
                }
            }
            return Ok(getAllResult.Value);
        }

        [HttpGet(GetUserByUserName, Name = nameof(GetUserByUserNameAsync))]
        public async Task<IActionResult> GetUserByUserNameAsync(string userName)
        {
            Result<BankUser> getResult = await _userRepository.GetByUserNameAsync(userName);
            if (getResult.Succeeded == false)
            {
                switch (getResult.ResultType)
                {
                    case ResultType.NotFound:
                        return NotFound();
                    case ResultType.InvalidData:
                        return BadRequest(getResult.Message);
                    case ResultType.DataStoreError:
                        return Conflict(getResult.Message);
                    case ResultType.Duplicate:
                        return Conflict(getResult.Message);
                    default:
                        return StatusCode(500);
                }
            }
            return Ok(getResult.Value);
        }

        [HttpPost(PostCreateUser, Name = nameof(PostCreateUserAsync))]
        public async Task<IActionResult> PostCreateUserAsync([FromBody] BankUser user)
        {
            Result<BankUser> createResult = await _userRepository.CreateUserAsync(user);
            if (createResult.Succeeded == false)
            {
                switch (createResult.ResultType)
                {
                    case ResultType.NotFound:
                        return NotFound();
                    case ResultType.InvalidData:
                        return BadRequest(createResult.Message);
                    case ResultType.DataStoreError:
                        return Conflict(createResult.Message);
                    case ResultType.Duplicate:
                        return Conflict(createResult.Message);
                    default:
                        return StatusCode(500);
                }
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
                switch (updateResult.ResultType)
                {
                    case ResultType.NotFound:
                        return NotFound();
                    case ResultType.InvalidData:
                        return BadRequest(updateResult.Message);
                    case ResultType.DataStoreError:
                        return Conflict(updateResult.Message);
                    case ResultType.Duplicate:
                        return Conflict(updateResult.Message);
                    default:
                        return StatusCode(500);
                }
            }
            return Ok(updateResult.Value);
        }

        [HttpDelete(DeleteBankUser, Name = nameof(DeleteBankUserAsync))]
        public async Task<IActionResult> DeleteBankUserAsync(string userName, string id)
        {
            Result<BankUser> deleteResult = await _userRepository.DeleteUserAsync(userName, id);            
            if (deleteResult.Succeeded == false)
            {
                switch (deleteResult.ResultType)
                {
                    case ResultType.NotFound:
                        return NotFound(deleteResult.Message);
                    case ResultType.InvalidData:
                        return BadRequest(deleteResult.Message);
                    case ResultType.DataStoreError:
                        return Conflict(deleteResult.Message);
                    case ResultType.Duplicate:
                        return Conflict(deleteResult.Message);
                    default:
                        return StatusCode(500);
                }
            }
            return Ok(deleteResult.Value);
        }
    }
}
