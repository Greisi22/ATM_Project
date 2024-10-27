using CleanArchitecture.Application.Models;
using CleanArchitecture.Application.Services.LogIn;
using CleanArchitecture.Application.Services.SignUp;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LogInController :BaseController
{
    private readonly ILogger<LogInController> _logger;
    private readonly ILogInService _logInService;

    public LogInController(ILogger<LogInController> logger, ILogInService logInService)
    {
        _logger = logger;
        _logInService = logInService;
    }


    [HttpPost]

    public async Task<IActionResult> LogInAsync([FromBody] LogInDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var cancellationToken = HttpContext.RequestAborted;
        var result = await _logInService.LoginAsync(request, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result);
    
}
}
