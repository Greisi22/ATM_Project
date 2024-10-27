using CleanArchitecture.Application.Models;
using CleanArchitecture.Application.Services.SignUp;
using CleanArchitecture.Application.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class SignUpController : BaseController
{
    private readonly ILogger<SignUpController> _logger;
    private readonly ISignUpService _signUpService;

    public SignUpController (ILogger<SignUpController> logger, ISignUpService signUpService)
    {
        _logger = logger;
        _signUpService = signUpService;
    }

    [HttpPost]
    public async Task<IActionResult> SignUpAsync( [FromBody] SignUpDTO request)
    {
         if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var cancellationToken = HttpContext.RequestAborted;
        var result = await _signUpService.SignUpAsync(request, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result);
    }


    
}
