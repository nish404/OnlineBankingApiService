using Bank;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;

namespace OnlineBankingApiService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<BankController> _logger;

        public BankController(IAccountRepository accountRepository, ILogger<BankController> logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }

        [HttpGet("/accounts", Name = nameof(GetAllAccountsAsync))]
        public async Task<IActionResult> GetAllAccountsAsync()
        {
            Result<List<Account>> getAllResult = await _accountRepository.GetAllAccountsAsync();
            if(getAllResult.Succeeded == false)
            {
                return BadRequest();
            }
            return Ok(getAllResult.Value);
        }

        [HttpGet("/number2", Name = nameof(GetNumber2))]
        public IActionResult GetNumber2()
        {
            return Ok(5);
        }
    }
}