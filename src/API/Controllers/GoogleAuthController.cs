using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Services.Google;
using CleanArchitecture.Application.Services.Token;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class GoogleAuthController : ControllerBase
    {
        private readonly IGoogleAuthService _googleAuthService;
        private readonly ITokenService _tokenService;

        public GoogleAuthController(IGoogleAuthService googleAuthService, ITokenService tokenService)
        {
            _googleAuthService = googleAuthService;
            _tokenService = tokenService;
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] string token, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return BadRequest("Token is required.");
            }

            try
            {
                // Validate Google token and get the user
                var user = await _googleAuthService.ValidateGoogleTokenAsync(token, cancellationToken);

                if (user == null)
                {
                    return Unauthorized("User not found or could not be created.");
                }

                // Generate JWT for the authenticated user
                var jwtToken = _tokenService.GenerateToken(user);

                return Ok(new
                {
                    token = jwtToken,
                    email = user.Email,
                    role = user.Role
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid Google token");
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
