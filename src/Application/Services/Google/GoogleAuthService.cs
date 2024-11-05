using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Services.Google
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ILogger<GoogleAuthService> _logger;

        public GoogleAuthService(IApplicationDbContext applicationDbContext, ILogger<GoogleAuthService> logger)
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
        }

        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenAsync(string token)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(token);
                return payload;
            }
            catch (InvalidJwtException ex)
            {
                // Log the exception and return null for invalid tokens
                _logger.LogError(ex, "Invalid Google token");
                return null;
            }
        }

        public async Task<User> ValidateGoogleTokenAsync(string token, CancellationToken cancellationToken)
        {
            var payload = await VerifyGoogleTokenAsync(token);
            if (payload == null)
            {
                throw new UnauthorizedAccessException("Invalid Google token");
            }

            // Use the payload to get the user's email and Google ID
            return await GetOrCreateUserAsync(payload.Email, payload.Subject, cancellationToken);
        }

        public async Task<User> GetOrCreateUserAsync(string email, string googleId, CancellationToken cancellationToken)
        {
            // Check if the user already exists
            var user = await _applicationDbContext.EntitySet<User>()
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            if (user == null)
            {
                // Create a new user if not found
                user = new User
                {
                    Email = email,
                    GoogleId = googleId, 
                    Role = UserRole.AtmUser 
                };

                await _applicationDbContext.EntitySet<User>().AddAsync(user, cancellationToken);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);
            }

            return user; // Return the user (either existing or newly created)
        }
    }
}
