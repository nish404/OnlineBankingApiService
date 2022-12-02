using Bank;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;

namespace OnlineBankingApiService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<AccountsController> _logger;

        // Routes
        private const string GetAllAccounts = "/accounts";
        private const string GetAllAccountsByUserName = "/accounts/{userName}";
        private const string PostCreateAccount = "/accounts";
        private const string PutUpdateAccount = "/accounts/{id}";
        private const string DeleteAccount = "/accounts/{userName}/{id}";

        public AccountsController(IAccountRepository accountRepository, ILogger<AccountsController> logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }
     
        [HttpGet(GetAllAccounts, Name = nameof(GetAllAccountsAsync))]
        public async Task<IActionResult> GetAllAccountsAsync()
        {
            Result<List<Account>> getAllResult = await _accountRepository.GetAllAccountsAsync();
            if(getAllResult.Succeeded == false)
            {
                return BadRequest();
            }
            return Ok(getAllResult.Value);
        }

        [HttpGet(GetAllAccountsByUserName, Name = nameof(GetAllAccountsByUserNameAsync))]
        public async Task<IActionResult> GetAllAccountsByUserNameAsync(string userName)
        {
            Result<List<Account>> getAllByUserNameResult = await _accountRepository.GetAllByUsernameAsync(userName);
            if (getAllByUserNameResult.Succeeded == false)
            {
                return BadRequest();
            }
            return Ok(getAllByUserNameResult.Value);
        }

        [HttpPost(PostCreateAccount, Name = nameof(PostCreateAccountAsync))]
        public async Task<IActionResult> PostCreateAccountAsync([FromBody] Account account)
        {
            Result<Account> createResult = await _accountRepository.CreateAccountAsync(account);
            if (createResult.Succeeded == false)
            {
                return BadRequest();
            }
            return Ok(createResult.Value);
        }

        [HttpPut(PutUpdateAccount, Name = nameof(PutUpdateAccountAsync))]
        public async Task<IActionResult> PutUpdateAccountAsync(string id, [FromBody] Account account)
        {
            if(id != account.Id)
            {
                return BadRequest("Parameter 'id' does not match id from request body");
            }

            Result<Account> updateResult = await _accountRepository.UpdateAccountAsync(account);
            if (updateResult.Succeeded == false)
            {
                return BadRequest();
            }
            return Ok(updateResult.Value);
        }

        [HttpDelete(DeleteAccount, Name = nameof(DeleteUserAccountAsync))]
        public async Task<IActionResult> DeleteUserAccountAsync(string userName, string id)
        {
            Result<Account> deleteResult = await _accountRepository.DeleteAccountAsync(id, userName);
            if (deleteResult.Succeeded == false)
            {
                return BadRequest();
            }
            return Ok(deleteResult.Value);
        }
    }
}
