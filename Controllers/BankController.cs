using Bank;
using DataAccess;
using Logic;
using Microsoft.AspNetCore.Mvc;
using OnlineBankingProject.Common.Models;
using System.Collections.Generic;

namespace OnlineBankingApiService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountLogic _accountLogic;
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<AccountsController> _logger;

        // Routes
        private const string GetAllAccounts = "/accounts";
        private const string GetAllAccountsByUserName = "/accounts/{userName}";
        private const string GetAccount = "/accounts/{userName}/{id}";
        private const string PostCreateAccount = "/accounts";
        private const string PutUpdateAccount = "/accounts/{id}";
        private const string DeleteAccount = "/accounts/{userName}/{id}";
        private const string PutWithdraw = "/accounts/{userName}/{id}/withdraw";
        private const string PutDeposit = "/accounts/{userName}/{id}/deposit";
        private const string PutTransfer = "/accounts/{userName}/{id}/transfer";
        private const string GetBalance = "/accounts/{userName}/{id}/balance";

        public AccountsController(IAccountLogic accountLogic, IAccountRepository accountRepository, ILogger<AccountsController> logger)
        {
            _accountLogic = accountLogic;
            _accountRepository = accountRepository;
            _logger = logger;
        }

        [HttpGet(GetAccount, Name = nameof(GetAccountAsync))]
        public async Task<IActionResult> GetAccountAsync(string userName, string id)
        {
            Result<Account> getAccount = await _accountRepository.GetByIdAsync(id);
            if (getAccount.Succeeded == false)
            {
                switch (getAccount.ResultType)
                {
                    case ResultType.NotFound:
                        return NotFound(getAccount.Message);
                    case ResultType.InvalidData:
                        return BadRequest(getAccount.Message);
                    case ResultType.DataStoreError:
                        return Conflict(getAccount.Message);
                    case ResultType.Duplicate:
                        return Conflict(getAccount.Message);
                    default:
                        return StatusCode(500);
                }
            }
            return Ok(getAccount.Value);
        }

        [HttpGet(GetAllAccounts, Name = nameof(GetAllAccountsAsync))]
        public async Task<IActionResult> GetAllAccountsAsync()
        {
            Result<List<Account>> getAllResult = await _accountRepository.GetAllAccountsAsync();
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

        [HttpGet(GetAllAccountsByUserName, Name = nameof(GetAllAccountsByUserNameAsync))]
        public async Task<IActionResult> GetAllAccountsByUserNameAsync(string userName)
        {
            Result<List<Account>> getAllByUserNameResult = await _accountRepository.GetAllByUsernameAsync(userName);
            if (getAllByUserNameResult.Succeeded == false)
            {
                switch (getAllByUserNameResult.ResultType)
                {
                    case ResultType.NotFound:
                        return NotFound();
                    case ResultType.InvalidData:
                        return BadRequest(getAllByUserNameResult.Message);
                    case ResultType.DataStoreError:
                        return Conflict(getAllByUserNameResult.Message);
                    case ResultType.Duplicate:
                        return Conflict(getAllByUserNameResult.Message);
                    default:
                        return StatusCode(500);
                }
            }
            return Ok(getAllByUserNameResult.Value);
        }

        [HttpPost(PostCreateAccount, Name = nameof(PostCreateAccountAsync))]
        public async Task<IActionResult> PostCreateAccountAsync([FromBody] Account account)
        {
            Result<Account> createResult = await _accountRepository.CreateAccountAsync(account);
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

        [HttpPut(PutUpdateAccount, Name = nameof(PutUpdateAccountAsync))]
        public async Task<IActionResult> PutUpdateAccountAsync(string id, [FromBody] Account account)
        {
            if (id != account.Id)
            {
                return BadRequest("Parameter 'id' does not match id from request body");
            }

            Result<Account> updateResult = await _accountRepository.UpdateAccountAsync(account);
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

        [HttpDelete(DeleteAccount, Name = nameof(DeleteUserAccountAsync))]
        public async Task<IActionResult> DeleteUserAccountAsync(string userName, string id)
        {
            Result<Account> deleteResult = await _accountRepository.DeleteAccountAsync(id, userName);
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

        [HttpPut(PutWithdraw, Name = nameof(PutWithdraw))]
        public async Task<IActionResult> PutWithdrawAsync(string userName, string id, [FromBody] AccountWithdrawRequest accountWithdrawRequest)
        {
            if (accountWithdrawRequest == null)
            {
                return BadRequest("Invalid or missing accountWithdrawRequest");
            }
            Result<Account> withDrawResult = await _accountLogic.WithdrawAmount(id, accountWithdrawRequest.pin, accountWithdrawRequest.amount);
            if (withDrawResult.Succeeded == false)
            {
                switch (withDrawResult.ResultType)
                {
                    case ResultType.NotFound:
                        return NotFound(withDrawResult.Message);
                    case ResultType.InvalidData:
                        return BadRequest(withDrawResult.Message);
                    case ResultType.DataStoreError:
                        return Conflict(withDrawResult.Message);
                    case ResultType.Duplicate:
                        return Conflict(withDrawResult.Message);
                    default:
                        return StatusCode(500);
                }
            }
            return Ok(withDrawResult.Value);
        }

        [HttpPut(PutDeposit, Name = nameof(PutDeposit))]
        public async Task<IActionResult> PutDepositAsync(string userName, string id, [FromBody] AccountDepositRequest accountDepositRequest)
        {
            if (accountDepositRequest == null)
            {
                return BadRequest("Invalid or missing accountDepositRequest");
            }
            Result<Account> depositResult = await _accountLogic.DepositAmount(id, accountDepositRequest.amount, accountDepositRequest.accountNumber);
            if (depositResult.Succeeded == false)
            {
                switch (depositResult.ResultType)
                {
                    case ResultType.NotFound:
                        return NotFound(depositResult.Message);
                    case ResultType.InvalidData:
                        return BadRequest(depositResult.Message);
                    case ResultType.DataStoreError:
                        return Conflict(depositResult.Message);
                    case ResultType.Duplicate:
                        return Conflict(depositResult.Message);
                    default:
                        return StatusCode(500);
                }
            }
            return Ok(depositResult.Value);
        }

        [HttpPut(PutTransfer, Name = nameof(PutTransfer))]
        public async Task<IActionResult> PutTransferAsync(string userName, string id, [FromBody] AccountTransferRequest accountTransferRequest)
        {
            if (accountTransferRequest == null)
            {
                return BadRequest("Invalid or missing accountDepositRequest");
            }
            Result<Account> transferResult = await _accountLogic.TransferAmount(id, accountTransferRequest.sourceAccountNumber, accountTransferRequest.sourcePin, accountTransferRequest.destinationAccountNumber, accountTransferRequest.amount);
            if (transferResult.Succeeded == false)
            {
                switch (transferResult.ResultType)
                {
                    case ResultType.NotFound:
                        return NotFound(transferResult.Message);
                    case ResultType.InvalidData:
                        return BadRequest(transferResult.Message);
                    case ResultType.DataStoreError:
                        return Conflict(transferResult.Message);
                    case ResultType.Duplicate:
                        return Conflict(transferResult.Message);
                    default:
                        return StatusCode(500);
                }
            }
            return Ok(transferResult.Value);
        }

        [HttpGet(GetBalance, Name = nameof(GetBalance))]
        public async Task<IActionResult> GetBalanceAsync(string userName, string id)
        {
            Result<Account> getAccountResult = await _accountRepository.GetByIdAsync(id);
            if (getAccountResult.Succeeded == false)
            {
                switch (getAccountResult.ResultType)
                {
                    case ResultType.NotFound:
                        return NotFound();
                    case ResultType.InvalidData:
                        return BadRequest(getAccountResult.Message);
                    case ResultType.DataStoreError:
                        return Conflict(getAccountResult.Message);
                    case ResultType.Duplicate:
                        return Conflict(getAccountResult.Message);
                    default:
                        return StatusCode(500);
                }
            }
            return Ok(getAccountResult.Value.Amount);
        }
    }
}
