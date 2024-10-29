using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Application.Models;
using CleanArchitecture.Application.Services.Account;
using CleanArchitecture.Application.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize(Policy = "RequireAdminRole")]
[Route("bankAccount")]
public class BankAccountController: BaseController
{
    private readonly ILogger<UserController> _logger;
    private readonly IBankAccountService _bankaccountservice;
    public BankAccountController(ILogger<UserController> logger, IBankAccountService bankaccountservice)
    {
        _logger = logger;
        _bankaccountservice = bankaccountservice;
    }


    [HttpPost]

    public async Task<IActionResult> createBankAccount([FromBody] BankAccountDto bankAccountDto, CancellationToken cancellationToken)
    {

        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

       var createdAccount = await _bankaccountservice.CreateAccount(bankAccountDto, cancellationToken);

        return Ok(createdAccount);
    }

    [HttpGet]

    public async Task<IActionResult> getAll()
    {
        var result = _bankaccountservice.GetAll();

        return Ok(result);
    }

}
