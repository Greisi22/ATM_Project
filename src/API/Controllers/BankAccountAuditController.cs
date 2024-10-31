using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Application.Services.Account;
using CleanArchitecture.Application.Services.BankAccountAudit;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("auditlog")]
public class BankAccountAuditController : BaseController
{
    private readonly ILogger<UserController> _logger;
    private readonly IBankAccountAuditService _bankAccountAuditService;

    public BankAccountAuditController(ILogger<UserController> logger, IBankAccountAuditService bankAccountAuditService)
    {
        _logger = logger;
        _bankAccountAuditService = bankAccountAuditService;
    }

    [Microsoft.AspNetCore.Authorization.Authorize(Policy = "RequireAdminOrUserRole")]
    [HttpGet("/SpecificBankAccount/{id}")]
    public async Task<IActionResult> GetAuditBankAccountId(Guid id)
    {
        var result = await _bankAccountAuditService.GetAuditBankAccountId(id);

        return Ok(result);
    }

    [Microsoft.AspNetCore.Authorization.Authorize(Policy = "RequireAdminRole")]
    [HttpGet("/SpecificUser/{id}")]
    public async Task<IActionResult> GetAuditUserId(Guid id)
    {
        var result = await _bankAccountAuditService.GetAuditUserId(id);

        return Ok(result);
    }

    [Microsoft.AspNetCore.Authorization.Authorize(Policy = "RequireAdminRole")]
    [HttpGet("/BankAccountAuditLog")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _bankAccountAuditService.GetAll();

        return Ok(result);
    }

}
