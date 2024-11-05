
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Models;
using CleanArchitecture.Application.Services.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace CleanArchitecture.Application.Services.LogIn;
public class LogInService : ILogInService
{

    private readonly IApplicationDbContext _applicationDbContext;
    private readonly ITokenService _tokenService;
    public LogInService(IApplicationDbContext applicationDbContext, ITokenService tokenService)
    {
        _applicationDbContext = applicationDbContext;
        _tokenService = tokenService;
    }

    public async Task<LogInResultDto> LoginAsync(LogInDto request, CancellationToken cancellationToken)
    {
        var user = await _applicationDbContext.EntitySet<User>()
                 .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null)
        {
            return new LogInResultDto { Success = false, Errors = new[] { "Invalid username or password." } };
        }

        var passwordHasher = new PasswordHasher<User>();
        
        var result = passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);


        if (result == PasswordVerificationResult.Failed)
        {
            return new LogInResultDto { Success = false, Errors = new[] { "Invalid username or password." } };


        }
        if (_tokenService == null)
            {
                throw new InvalidOperationException("TokenService is not initialized.");
            }
        
        try
        {
            var token = _tokenService.GenerateToken(user);
            return new LogInResultDto { Success = true, Token = token, role = user.Role  };
        }
        catch (Exception ex)
        {

            return new LogInResultDto { Success = false, Errors = new[] { ex.Message } };
        }
    }
}


