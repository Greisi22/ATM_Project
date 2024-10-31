using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Application.Models;
using CleanArchitecture.Application.Services.Account;
using CleanArchitecture.Application.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

[ApiController]
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

    [Microsoft.AspNetCore.Authorization.Authorize(Policy = "RequireAdminRole")]
    [HttpPost]
    public async Task<IActionResult> CreateBankAccount([FromBody] BankAccountDto bankAccountDto, CancellationToken cancellationToken)
    {

        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

       var createdAccount = await _bankaccountservice.CreateAccount(bankAccountDto, cancellationToken);

        return Ok(createdAccount);
    }

    [Microsoft.AspNetCore.Authorization.Authorize(Policy = "RequireAdminOrUserRole")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _bankaccountservice.GetById(id);

        return Ok(result);
    }


    [Microsoft.AspNetCore.Authorization.Authorize(Policy = "RequireAdminRole")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _bankaccountservice.GetAll();

        return Ok(result);
    }

    [Microsoft.AspNetCore.Authorization.Authorize(Policy = "RequireAtmUserRole")]
    [HttpGet("myBankAccounts")]
    public async Task<IActionResult> GetBankAccounts()
    {
        var result = await _bankaccountservice.GetBankAccounts();

        return Ok(result);
    }

    [Microsoft.AspNetCore.Authorization.Authorize(Policy = "RequireAdminRole")]
    /// <summary>
    /// Get Bank accounts for specific users
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("user")]
    public async Task<IActionResult> GetBanckAccountsByUserId([FromQuery] Guid userId)
    {
        var result = await _bankaccountservice.GetAccountsByUserID(userId);

        return Ok(result);
    }

    [Microsoft.AspNetCore.Authorization.Authorize(Policy = "RequireAtmUserRole")]
    [HttpPut("{id}/withdraw")]
    public async Task<IActionResult> Withdraw([FromBody] double withdrawAmount, [FromRoute] Guid id, CancellationToken cancellationToken)
    {

        var createdAccount = await _bankaccountservice.WithdrawBalance(withdrawAmount,id, cancellationToken);

        return Ok(createdAccount);
    }



    [Microsoft.AspNetCore.Authorization.Authorize(Policy = "RequireAtmUserRole")]
    [HttpPut("{id}/deposit")]
    public async Task<IActionResult> Deposit([FromBody] double depositAmount, [FromRoute] Guid id, CancellationToken cancellationToken)
    {

        var createdAccount = await _bankaccountservice.DepositBalance(depositAmount, id, cancellationToken);

        return Ok(createdAccount);
    }
}
